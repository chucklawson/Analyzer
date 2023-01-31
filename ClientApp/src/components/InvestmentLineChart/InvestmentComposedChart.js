import React from "react";
import {
    ComposedChart,
    Line,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    Legend
  } from 'recharts';


const InvestmentComposedChar = (props) => {

    return (
        <div>
          <ComposedChart
            width={props.width}
            height={props.height}
            data={props.data}
                margin={props.margin}>

            <CartesianGrid stroke="#f5f5f5" />
                {/*<XAxis dataKey="name" scale="band" />*/}
                <XAxis dataKey="name" interval={3} angle={60} dx={20} scale="band" />
                <YAxis type="number" domain={['auto','auto']} />
            <Tooltip />
            <Legend />
                <Area type="monotone" dataKey="dailyPrices" fill="#a3ee9e" stroke="#12b5098" />
            {/*<Bar dataKey="pv" barSize={20} fill="#413ea0" />*/}
                <Line type="monotone" dataKey="simpleMovingAverage" strokeWidth={props.lineWidth} stroke="#095cb5" />
                <Line type="monotone" dataKey="expMovingAverage" strokeWidth={props.lineWidth} stroke="#b50909" />
            {/*<Scatter dataKey="cnt" fill="red" />*/}
          </ComposedChart>
        </div>
      );
};

export default InvestmentComposedChar;