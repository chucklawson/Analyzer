import React, { useEffect, useState } from 'react';
import './App.css';
import TickerWebSocket from './components/webSocket/TickerWebSocket';


function App() {

    const [headerValue, setHeaderValue] = useState("Performance");
    useEffect(() => {
        console.log("Running useEffect in APP.js.....")
    }, [headerValue])

    

    const onSetHeader = (headerValueIn) => {
        setHeaderValue(headerValueIn);
    }

    return (
        <div className="App">
            
            <header className="bg-green-100 text-green-600 text-3xl font-bold underline h-18">
                {headerValue}
            </header>

            <TickerWebSocket onSetHeader={onSetHeader}></TickerWebSocket>

            <footer className="App-footer">
                {/*Footer*/}
            </footer>
        </div>
    );
}

export default App;
