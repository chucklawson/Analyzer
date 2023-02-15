import React, { useState } from 'react';

const TradingRangeIndicator = props => {
  


    return (
        <div className="h-1 mt-5 mb-20">       
                
                <label class="form-label">{props.heading}</label>
                <div>
                    <span className="inline-block mr-1"> {props.lowRangeValue} </span>  
                
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
                        value={props.rangeValue}
                        id="customRange1"
                        disabled
                    />
                    <span className="inline-block ml-1">{props.highRangeValue}</span>  
                </div>

                <div className="text-gray-600 font-normal text-xs mt-3">
                    Open: ${props.topOfBookData[0].open}, Low: ${props.topOfBookData[0].low}, High: ${props.topOfBookData[0].high}, Last: ${props.topOfBookData[0].last}
                </div>
        </div> 
  );
};

export default TradingRangeIndicator;