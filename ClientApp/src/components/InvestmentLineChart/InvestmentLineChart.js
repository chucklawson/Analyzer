import React from "react";

import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    Legend
  } from "recharts";

  const InvestmentLineChart =(props)=>{
    return (
      <div>
        <LineChart
          width={props.width}
          height={props.height}
          data={props.data}
          margin={props.margin}>

          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="name" interval={5} angle={60} dx={3} />
          <YAxis />
          <Tooltip />
          <Legend />
          <Line
            type="monotone"
                    dataKey="dailyPrices"
            strokeWidth={props.lineWidth}
            stroke="#8884d8"
            activeDot={{ r: 8 }}
          />
                <Line type="monotone" dataKey="simpleMovingAverage" strokeWidth={props.lineWidth} stroke="#82ca9d" />
                <Line type="monotone" dataKey="expMovingAverage" strokeWidth={props.lineWidth} stroke="#2f2680" />
        </LineChart>
      </div>
    );

  };
  
  export default InvestmentLineChart; 