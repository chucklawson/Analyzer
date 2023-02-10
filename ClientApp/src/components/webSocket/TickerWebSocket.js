
import React, { useState, useEffect, useRef } from 'react';
import TickerInput from '../TickerInput/TickerInput';
import InvestmentLineChart from '../InvestmentLineChart/InvestmentLineChart'
import InvestmentComposedChar from '../InvestmentLineChart/InvestmentComposedChart';
import styles from './TickerWebSocket.module.css';

const TickerWebSocket = () => {
    const [val, setVal] = useState(null);
    const OBTAIN_TICKER_VALUES = "OBTAIN_TICKER_VALUES";
    const OBTAIN_TOP_OF_BOOK = "OBTAIN_TOP_OF_BOOK";
    const OBTAIN_CSV_TICKER_DATA = "OBTAIN_CSV_TICKER_DATA";
    const ws = useRef(null);
    const [tickerToGet, setTickerToGet] = useState('');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [updateTickerValue, setUpdateTickerValue] = useState(false);

    const [graphData, setGraphData] = useState({});
    const [topOfBookData, setTopOfBookData] = useState([{}]);
    const [csvTickerData, setCsvTickerData] = useState([{}]);
    const widthOfStroke = 2;



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
        }
        else {
            console.log('Reset: updateTickerValue to false: ' + updateTickerValue);
        }
    }, [tickerToGet, startDate, endDate, updateTickerValue]);

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
    };

    return <div className={`${styles['basic-control']}`}>Value: {val}
        <TickerInput onTickerValue={onTickerChangeHandler} currentTicker={tickerToGet} startDate={startDate} endDate={endDate} ></TickerInput>
        <div className="bg-green-100 text-1xl font-bold underline h-5">
            OPEN ${topOfBookData[0].open},   HIGH ${topOfBookData[0].high},   LOW ${topOfBookData[0].low},   LAST ${topOfBookData[0].last}
        </div>
        <InvestmentComposedChar
            width={700}
            height={275}
            data={graphData}
            margin={{
                top: 5,
                right: 30,
                left: 20,
                bottom: 5
            }}
            lineWidth={widthOfStroke}>

        </InvestmentComposedChar>
        

        {/*
        <InvestmentLineChart
            width={800}
            height={400}
            data={graphData}
            margin={{
                top: 5,
                right: 30,
                left: 20,
                bottom: 5
            }}>
        </InvestmentLineChart>
        */}

    </div>;
};

    

    

export default TickerWebSocket;