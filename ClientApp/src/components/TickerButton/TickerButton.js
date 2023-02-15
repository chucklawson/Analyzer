import React, { useState } from 'react';

const TickerButton = props => {
  
 const onSelectHandler = (event)=> {
     props.selectTickerButtonHandler(event.target.innerText);
  };


    return (
        < button className='bg-green-400 p-1 rounded-md ml-2 mr-2 mt-1 text-white hover:text-black transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 hover:bg-green-200 duration-300' style={{ width: '125px' }} onClick={onSelectHandler}>
             {props.ticker}
        </button>
  );
};

export default TickerButton;