import React, { useEffect, useState } from 'react';
import './App.css';
import TickerWebSocket from './components/webSocket/TickerWebSocket';


function App() {

    const [headerValue, setHeaderValue] = useState("Performance");
    const [todaysPercentageChange, setTodaysPercentageChange] = useState(0.0);
    const [isTodaysChangePositive, setIsTodaysChangePositive] = useState(true);
    useEffect(() => {
        console.log("Running useEffect in APP.js.....")
    }, [headerValue, todaysPercentageChange])
    

    const onSetHeader = (headerValueIn) => {
        setHeaderValue(headerValueIn);
    }

    const onSetTodaysPercentageChange = (percentageChange, isChnagePositive) => {
        setTodaysPercentageChange(percentageChange);
        setIsTodaysChangePositive(isChnagePositive);
    }

    return (
        <div className="App">
            
            <header className="bg-green-100 text-green-600 text-3xl font-bold h-18 justify-items-center">
                <div>
                    {headerValue}
                </div>
                <div>
                {isTodaysChangePositive === true ?
                    <div className='text-green-600 text-3xl font-bold'>
                        Today's Change: {todaysPercentageChange} %
                    </div> :
                    <div className='text-red-600 text-3xl font-bold'>
                        Today's Change: {todaysPercentageChange} %
                    </div>
                    }
                </div>
            </header>

            <TickerWebSocket onSetHeader={onSetHeader} onSetTodaysPercentageChange={onSetTodaysPercentageChange} ></TickerWebSocket>

            <footer className="App-footer">
                {/*Footer*/}
            </footer>
        </div>
    );
}

export default App;
