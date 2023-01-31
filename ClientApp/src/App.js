import React, { useEffect } from 'react';
import './App.css';
import TickerWebSocket from './components/webSocket/TickerWebSocket';


function App() {

    useEffect(() => {
        console.log("Running useEffect in APP.js.....")
    }, [])


    return (
        <div className="App">
            
            <header className="App-header">
                Analyzer
            </header>

            <TickerWebSocket></TickerWebSocket>

            <footer className="App-footer">
                Footer
            </footer>
        </div>
    );
}

export default App;
