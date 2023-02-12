import React, { useEffect } from 'react';
import './App.css';
import TickerWebSocket from './components/webSocket/TickerWebSocket';


function App() {

    useEffect(() => {
        console.log("Running useEffect in APP.js.....")
    }, [])


    return (
        <div className="App">
            
            <header className="bg-green-100 text-green-600 text-3xl font-bold underline h-18">
                Analyzer
            </header>

            <TickerWebSocket></TickerWebSocket>

            <footer className="App-footer">
                {/*Footer*/}
            </footer>
        </div>
    );
}

export default App;
