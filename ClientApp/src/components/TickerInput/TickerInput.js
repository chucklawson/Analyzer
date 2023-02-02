import React, { useState } from 'react';

import Button from '../../UI/Button/Button';
//import styled from 'styled-components';

import styles from './TickerInput.module.css';

const TickerInput = props => {
  const [enteredValue, setEnteredValue] = useState('');
    const [isValid, setIsValid] = useState(true);
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');

    const tickerInputChangeHandler = event => {
       // event.preventDefault();
    if(event.target.value.trim().length>0)
    {
      setIsValid(true);
    }
    setEnteredValue(event.target.value);
  };

  const formSubmitHandler = event => {
    event.preventDefault();
    if(enteredValue.trim().length===0)
    {
      setIsValid(false);
      return
    }
      console.log("Entered value:" + enteredValue);
      props.onTickerValue(enteredValue, startDate,endDate);
  };

    const startDateChangeHandler = (event) => {
        setStartDate(event.target.value);
    };

    const endDateChangeHandler = (event) => {
        setEndDate(event.target.value);
    };

  return (
    <form onSubmit={formSubmitHandler}>
      {/*<FormControl invalid={!isValid}>*/}
      <div className = {`${styles['form-control']} ${!isValid && styles.invalid}`}>
              <label>Ticker: {props.currentTicker} Start date: {props.startDate} End date: {props.endDate}</label>
              <input type="text" onChange={tickerInputChangeHandler}  style =  {{borderColor: !isValid ? 'red': 'black',
                  background: !isValid ? 'salmon' : 'white'
              }} />
              <div className='form-conrol__control'>
                  <label>{startDate}</label>
                  <input type='date' min='2019-01-01' max='2023-12-31' value={startDate} onChange={startDateChangeHandler} />
                  <label>{endDate}</label>
                  <input type='date' min='2019-01-01' max='2023-12-31' value={endDate} onChange={endDateChangeHandler} />
              </div>
      {/*</FormControl>*/}
      </div>
      <Button type="submit">Check Ticker</Button>
    </form>
  );
};

export default TickerInput;