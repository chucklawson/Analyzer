import React, { useState } from 'react';

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
      <div className='col-start-3 col-span-5 p-7 m-10'>
    <form onSubmit={formSubmitHandler}>      

          <div className='bg-green-300 p-5 m-8 rounded-lg'>

              <div className='grid grid-cols-4 justify-center'>
                        <div>
                            
                              <label>Ticker: {enteredValue}</label>
                              <input type="text" onChange={tickerInputChangeHandler} style={{
                                  borderColor: !isValid ? 'red' : 'black',
                                  background: !isValid ? 'salmon' : 'white'
                                            }} placeholder='VOO' />                            
                        </div>

                        <div>
                           
                                <label>Startdate: {startDate}</label>                            
                                <input type='date' min='2019-01-01' max='2023-12-31' value={startDate} onChange={startDateChangeHandler} />
                            
                        </div>

                  <div>
                      <label>Enddate: {endDate}</label>
                      <input type='date' min='2019-01-01' max='2023-12-31' value={endDate} onChange={endDateChangeHandler} />
                  </div>

                  <div className='justify-center'>
                            
                      < button className='bg-green-400 p-1 rounded-md ml-5 mt-5 text-white hover:text-black transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 hover:bg-green-200 duration-300' style={{ width: '125px' }} type='submit'>
                                Update Chart
                      </button>
                  </div>
              </div>

      </div>
          {/*<Button type="submit">Check Ticker</Button>*/}
            </form>
        </div>
  );
};

export default TickerInput;
