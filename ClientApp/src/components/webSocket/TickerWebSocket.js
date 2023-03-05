
import React, { useState, useEffect, useRef } from 'react';
import TickerInput from '../TickerInput/TickerInput';
import TickerButton from '../TickerButton/TickerButton';
import TradingRangeIndicator from'../TradingRangeIndicator/TradingRangeIndicator';
import InvestmentLineChart from '../InvestmentLineChart/InvestmentLineChart'
import InvestmentComposedChar from '../InvestmentLineChart/InvestmentComposedChart';

const TickerWebSocket = (props) => {
    
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
    const [firstReferenceClosingPrice, setFirstReferenceClosingPrice] = useState("");
    const [lastReferenceClosingPrice, setLastReferenceClosingPrice] = useState("");
    const [todaysGain, setTodaysGain] = useState(0.0);
    const [todaysPercentageGain, setTodaysPercentageGain] = useState(0.0);
    const [percentageChangeAcrossRange, setpercentageChangeAcrossRange] = useState(0.0);
    const [updateRangeValues, setUpdateRangeValues] = useState(false);
    const [gainIsPositive, setGainIsPositive] = useState(false);



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
        if ((typeof topOfBookData[0].low !== 'undefined') && (updateRangeValues ===true)) {
            setRangeValues(topOfBookData);
            setupdateRangeValuesToFalse();
        }
    }, [topOfBookData, updateRangeValues]);
    

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
                            setUpdateRangeValues(true);
                        }
                        else {
                            console.log("Not the calculatedPrices");
                        }                       
                    }
                    setFirstReferenceClosingPrice(obj.firstReferenceClosingPrice);
                    setLastReferenceClosingPrice(obj.lastReferenceClosingPrice);
                    //console.log("firstReferenceClosingPrice: " + obj.firstReferenceClosingPrice);
                    //console.log("lastReferenceClosingPrice: " + obj.lastReferenceClosingPrice);
                }

                if (obj.packageType === OBTAIN_TOP_OF_BOOK) {
                    for (var aTopOfBookProperty in obj) {
                        console.log("aTopOfBookProperty: " + aTopOfBookProperty)
                        
                        if (aTopOfBookProperty === 'topOfBookResponses') {                            
                            setTopOfBookData(obj[aTopOfBookProperty]);
                            console.log("Found topOfBookResponses and the length is: " + obj[aTopOfBookProperty].length);
                        }
                        else {
                            console.log("Not the topOfBookResponses");
                        }
                    }
                }

                if (obj.packageType === OBTAIN_CSV_TICKER_DATA) {
                    for (var aCsvProperty in obj) {
                        console.log("aCsvProperty: " + aCsvProperty)

                        if (aCsvProperty === 'csvTickerResponses') {                            
                            setCsvTickerData(obj[aCsvProperty]);
                            console.log("Found csvTickerResponses and the length is: " + obj[aCsvProperty].length);
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

    const setupdateRangeValuesToFalse = () => {
        setUpdateRangeValues(false);
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
            props.onSetHeader("Performance - " + tickerValue.trim());
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
        if (currentRange !== 0.0) {
            let percentage = ((currentDistanceFromLow / currentRange)*100.0);
            setRangeValue(percentage.toString());
        }
        /*
        console.log("lowValue" + lowValue);
        console.log("highValue" + highValue);
        console.log("lastValue" + lastValue);
        console.log("currentRange" + currentRange);
        console.log("lowValue" + lowValue);
        console.log("currentDistanceFromLow" + currentDistanceFromLow);
        */

        let firstReferencePrice = parseFloat(firstReferenceClosingPrice);
        let lastReferencePrice = parseFloat(lastReferenceClosingPrice);
        let todaysChange = (lastValue - lastReferencePrice).toFixed(2);
        let tempGain = false;
        setTodaysGain(todaysChange);
        if (todaysChange >= 0.0) {
            setGainIsPositive(true);
            tempGain = true;
        }
        else {
            setGainIsPositive(false);
        }
        let todaysPercentageGain = 0.0;
        if (lastReferencePrice !== 0.0) {
            todaysPercentageGain = ((todaysChange / lastReferencePrice) * 100.0).toFixed(2);
        }
        setTodaysPercentageGain(todaysPercentageGain);
        props.onSetTodaysPercentageChange(todaysPercentageGain, tempGain);

        let changeAcrossRange = lastValue - firstReferencePrice;
        let percentageChangeFullRange = 0.0;
        if (firstReferencePrice !== 0.0) {
            percentageChangeFullRange = ((changeAcrossRange / firstReferencePrice)*100.0).toFixed(2);
        }
        setpercentageChangeAcrossRange(percentageChangeFullRange); 
        /*
        console.log("firstReferencePrice" + firstReferencePrice);
        console.log("lastReferencePrice" + lastReferencePrice);
        console.log("todaysChange" + todaysChange);
        console.log("todaysPercentageGain" + todaysPercentageGain);
        console.log("changeAcrossRange" + changeAcrossRange);
        console.log("percentageChangeFullRange" + percentageChangeFullRange);
        */

        
    };


    const selectTickerButtonHandler = (tickerIn) => {
        setTickerToGet(tickerIn);
        setUpdateTickerValue(true);
        props.onSetHeader("Performance - " + tickerIn);
        console.log("tickerIn: " + tickerIn);
    }

    return <div className='bg-gray-100 grid grid-cols-9 gap-4'>

        <div className='col-start-1 col-span-2 bg-blue-100 m-5 rounded-md'>
            <TickerButton ticker='AMD' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='AAPL' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='AMZN' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='BKE' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='BRK-B' selectTickerButtonHandler={selectTickerButtonHandler} />   
            <TickerButton ticker='COF' selectTickerButtonHandler={selectTickerButtonHandler} />   
            <TickerButton ticker='COST' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='DHR' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='DIS' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='DVN' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='EL' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='EMR' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='EVRI' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='F' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='GOOGL' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='HAL' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='HON' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='LIN' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='LLY' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='MO' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='MS' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='MSFT' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='NUE' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='NVDA' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='OXY' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='SBUX' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='STZ' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='TJX' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='VOO' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='WFC' selectTickerButtonHandler={selectTickerButtonHandler} />
            <TickerButton ticker='WSM' selectTickerButtonHandler={selectTickerButtonHandler} />
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
            <div className='m1'>
                <TradingRangeIndicator heading="Last 12 Months" lowRangeValue={lowRangeValue} rangeValue={rangeValue} highRangeValue={highRangeValue} topOfBookData={topOfBookData} />
            </div >

            {/*<div className='m10 p8'>More stuff</div>*/}
            <div className='m1'>
                <TradingRangeIndicator heading="Today's Range" lowRangeValue={lowRangeValue} rangeValue={rangeValue} highRangeValue={highRangeValue} topOfBookData={topOfBookData} />
            </div>
            <div className='p-4 mt-6 mb-10'>
            {showChart === true ?
                    <div className='justify-items-start'> 
                        { gainIsPositive === true ?
                            <div>
                                <div className="text-1xl text-green-600 font-bold underline h-5 justify-items-start">
                                    Today's Gain: ${todaysGain}
                                </div>                        
                                <div className="text-1xl text-green-600 font-bold underline h-5">
                                    Today's % Gain: {todaysPercentageGain} %
                                </div>
                            </div> :
                            <div>
                                <div className="text-1xl text-red-600 font-bold underline h-5 justify-items-start">
                                Today's Gain: ${todaysGain}
                                </div>  
                                <div className="text-1xl text-red-600 font-bold underline h-5">
                                    Today's % Gain: {todaysPercentageGain} %
                                </div>
                           </div>}
                        {percentageChangeAcrossRange >= 0.0 ?
                            <div className="text-1xl text-green-600 font-bold underline h-5">
                                Range change % Gain: {percentageChangeAcrossRange} %
                            </div> :
                            <div className="text-1xl text-red-600 font-bold underline h-5">
                                Range change % Gain: {percentageChangeAcrossRange} %
                            </div>
                        }
                </div> :
                    <React.Fragment />}
            </div>

         </div>
        
    </div>
};

    

    

export default TickerWebSocket;