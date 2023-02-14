
import React, { useState, useEffect, useRef } from 'react';
import TickerInput from '../TickerInput/TickerInput';
import InvestmentLineChart from '../InvestmentLineChart/InvestmentLineChart'
import InvestmentComposedChar from '../InvestmentLineChart/InvestmentComposedChart';

const TickerWebSocket = () => {
    
    const OBTAIN_TICKER_VALUES = "OBTAIN_TICKER_VALUES";
    const OBTAIN_TOP_OF_BOOK = "OBTAIN_TOP_OF_BOOK";
    const OBTAIN_CSV_TICKER_DATA = "OBTAIN_CSV_TICKER_DATA";
    const ws = useRef(null);
    const [tickerToGet, setTickerToGet] = useState('');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [updateTickerValue, setUpdateTickerValue] = useState(false);
    const [showChart, setShowChart] = useState(false);

    const [graphData, setGraphData] = useState({});
    const [topOfBookData, setTopOfBookData] = useState([{}]);
    const [csvTickerData, setCsvTickerData] = useState([{}]);
    const widthOfStroke = 2;
    const buttonWidth = '100px';
    const [rangeValue, setRangeValue] = useState("50.0");
    const [lowRangeValue, setLowRangeValue] = useState("1.00");
    const [highRangeValue, setHighRangeValue] = useState("100");



    // start the websocket
    useEffect(() => {
        tickerSocket();
    }, []);


    

    // request ticker data
    useEffect(() => {
        if (updateTickerValue === true) {
            console.log('Sending ticker to Get: ' + tickerToGet);
            const sendJson = buildJsonToSend();
            console.log("sendJson: " + sendJson);
            ws.current.send(sendJson);

            const jsonAsTtopOfBookToSend = buildJsonToSendTopOfBook();
            ws.current.send(jsonAsTtopOfBookToSend);

            setUpdateTickerValueToFalse();
            setShowChart(true);
        }
        else {
            console.log('Reset: updateTickerValue to false: ' + updateTickerValue);
        }
    }, [tickerToGet, startDate, endDate, updateTickerValue]);

    useEffect(() => {  
        if (typeof topOfBookData[0].low != 'undefined') {
            setRangeValues(topOfBookData);
            }
    }, [topOfBookData]);

    const tickerSocket = () =>
    {
        // use this one for deployment to IIS on my local machine
        //const socket = new WebSocket("ws://localhost:8045/ws");
        // use this one for development and debugging
        const socket = new WebSocket("wss://localhost:7109/wss");

        socket.onopen = () => {
            console.log("opened webSocket");
        };

        socket.onclose = () => {
            console.log("closed the websocket");
        };

        socket.onmessage = (event) => {
            console.log("Event data: " + event.data);

            if (event.data && event.data.length > 1) {
                var obj = JSON.parse(event.data);
                //console.log("packageType: " + obj.packageType);
                if (obj.packageType === OBTAIN_TICKER_VALUES) {
                    for (var aProperty in obj) {
                        console.log("aProperty: " + aProperty)
                        //var packageType = obj.packageType
                        if (aProperty === 'calculatedPrices') {
                            console.log("Found calculatedPrices and the length is: " + obj[aProperty].length);
                            setGraphData(obj[aProperty]);
                        }
                        else {
                            console.log("Not the calculatedPrices");
                        }
                    }
                }

                if (obj.packageType === OBTAIN_TOP_OF_BOOK) {
                    for (var aProperty in obj) {
                        console.log("aProperty: " + aProperty)
                        
                        if (aProperty === 'topOfBookResponses') {                            
                            setTopOfBookData(obj[aProperty]);
                            console.log("Found topOfBookResponses and the length is: " + obj[aProperty].length);                                                       
                        }
                        else {
                            console.log("Not the topOfBookResponses");
                        }
                    }
                }

                if (obj.packageType === OBTAIN_CSV_TICKER_DATA) {
                    for (var aProperty in obj) {
                        console.log("aProperty: " + aProperty)

                        if (aProperty === 'csvTickerResponses') {                            
                            setCsvTickerData(obj[aProperty]);
                            console.log("Found csvTickerResponses and the length is: " + obj[aProperty].length);
                        }
                        else {
                            console.log("Not the csvTickerResponses");
                        }
                    }
                }
            }

            //setVal(event.data);
            //socket.send("Ok, the message got here");

            
        };

        ws.current = socket;       

        return () => {
            socket.close();
        };
    }

    const buildJsonToSend = () => {
        const tickerParameters = {
            operation: OBTAIN_TICKER_VALUES,
            ticker: tickerToGet,
            startDate: startDate,
            endDate: endDate
        }
        const jsonToSend = JSON.stringify(tickerParameters);
        //console.log("jsonToSend: " + jsonToSend);
        return jsonToSend;
            
    }

    const buildJsonToSendTopOfBook = () => {
        const topOfBookParameters = {
            operation: OBTAIN_TOP_OF_BOOK,
            ticker: tickerToGet
        }
        const jsonToSend = JSON.stringify(topOfBookParameters);
        //console.log("jsonToSend: " + jsonToSend);
        return jsonToSend;
    }

    const setUpdateTickerValueToFalse = () => {
        setUpdateTickerValue(false);
    }

    const onTickerChangeHandler = (tickerValue,startDate,endDate) => {
        if ((tickerValue.trim().length > 0)&&
            (tickerValue.trim().length > 0) &&
            (tickerValue.trim().length > 0))        {
            // looks like a couple of guys that need a reducer
            
            setTickerToGet(tickerValue.trim());
            setStartDate(startDate.trim());
            setEndDate(endDate.trim());
            setUpdateTickerValue(true);
            //console.log("tickerValue: " + tickerValue + ", startDate: " + startDate + ", endDate: " + endDate);            
        }
    }

    const setRangeValues = (topOfBookData) => {
        setLowRangeValue(topOfBookData[0].low.toString());
        setHighRangeValue(topOfBookData[0].high.toString());
        let lowValue = parseFloat(topOfBookData[0].low);
        let highValue = parseFloat(topOfBookData[0].high);
        let lastValue = parseFloat(topOfBookData[0].last);
        let currentRange = highValue - lowValue;
        let currentDistanceFromLow = lastValue - lowValue;
        if (currentRange != 0.0) {
            let percentage = ((currentDistanceFromLow / currentRange)*100.0);
            setRangeValue(percentage.toString());
        }
    };

    return <div className='bg-gray-100 grid grid-cols-9 gap-4'>

        <div className='col-start-1 col-span-2 bg-blue-100 m-5 rounded-md'>
            <button className='bg-green-400 p-1 rounded-md ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                AMD
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                AAPL
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                AMZN
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                BKE
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 5
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 6
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 7
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 8
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 9
            </button>
            <button className='bg-green-400 p-1 rounded-md ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 10
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 11
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 12
            </button>
            <button className='bg-green-400 p-1 rounded-md ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 13
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 14
            </button>
            <button className='bg-green-400 p-1 rounded-md  ml-2 mr-2 mt-1 text-white hover:text-black' style={{ width: '125px' }}>
                Button 15
            </button>
        </div>
        

        <div className='col-start-3 col-span-5'>
        <TickerInput  onTickerValue={onTickerChangeHandler} currentTicker={tickerToGet} startDate={startDate} endDate={endDate} ></TickerInput>
        
        {showChart === true ?
            <div className='justify-self-auto'>
                <div className="text-1xl text-green-600 font-bold underline h-5">
                    OPEN ${topOfBookData[0].open},   HIGH ${topOfBookData[0].high},   LOW ${topOfBookData[0].low},   LAST ${topOfBookData[0].last}
                </div>
                <div className='ml-20 mt-5'>
                    <InvestmentComposedChar
                            width={700}
                            height={275}
                            data={graphData}
                            margin={{
                                top: 5,
                                right: 30,
                                left: 50,
                                bottom: 5
                            }}
                            lineWidth={widthOfStroke}>

                    </InvestmentComposedChar>
                </div>
            </div>:
                <React.Fragment />}
        </div>

        <div className='col-start-8 col-span-2'>
           
            <div className="relative h-1">
                
                <label class="form-label">Today's Range</label>
                <div>
                    <span className="inline-block mr-1"> {lowRangeValue} </span>  
                
                    <input
                        type="range"
                        class="
                              inline-block
                              form-range
                              appearance-none
                              w-2/3
                              h-1
                              p-0
                              bg-gray-300
                              rounded-full
                              focus:outline-none focus:ring-0 focus:shadow-none
                            "
                        min="0"
                        max="100"
                        step="1.0"
                        value={rangeValue}
                        id="customRange1"
                        disabled
                    />
                    <span className="inline-block ml-1">{highRangeValue}</span>  
                </div>

                <div className="text-gray-600 font-normal text-xs mt-3">
                    Open: ${topOfBookData[0].open}, Low: ${topOfBookData[0].low}, High: ${topOfBookData[0].high}, Last: ${topOfBookData[0].last}
                </div>

            </div> 
        </div>


        


        <div className='col-start-9 col-span-1'>
            {/*
            <p className="relative h-1">
                <label for="customRange2" className="custom-range">Example range 2</label>
                <input type="range" className="custom-range" id="customRange2" bg-gray-300 min="0" max="100" value='15' disabled/>
            </p>
            */}
        </div>
        
    </div>
};

    

    

export default TickerWebSocket;