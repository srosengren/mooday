import './app.css';
import Calendar from 'react-calendar';
import React, { useState } from 'react';

function App() {

  const [selectedDate, setSelectedDate] = useState(new Date());

  return (
    <div className="app">
      <h1>Mooday</h1>
      <Calendar
        onChange={setSelectedDate}
        value={selectedDate}
      />
      <h2>{selectedDate.toDateString()}</h2>
    </div>
  );
}

export default App;
