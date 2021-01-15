import "./app.css";
import Calendar from "react-calendar";
import {useState, useEffect} from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Redirect,
  Route,
  useParams,
  useHistory
} from "react-router-dom";

const moodLevels = {
  5: {emoji: '😁'},
  4: {emoji: '😊'},
  3: {emoji: '😐'},
  2: {emoji: '😕'},
  1: {emoji: '😟'},
}

function dateToKey(date) {
  return date.getFullYear()+(`0${date.getMonth()+1}`.slice(-2))+date.getDate();
}

function calendarTileContent(date, allMoods) {
  const dateKey = dateToKey(date);
  return moodLevels[allMoods[dateKey]?.moodLevel]?.emoji
}

function Main() {
  const { dateParam } = useParams();
  const selectedDate = new Date(`${dateParam.substring(0,4)}-${dateParam.substring(4,6)}-${dateParam.substring(6,8)}`);
  const [allMoods, setAllMoods] = useState(false)
  const history = useHistory();

  function setSelectedDate(date) {
    history.push(dateToKey(date));
  }
  useEffect(() => {
    fetch('http://localhost:7071/api/moods')
      .then(response => response.json())
      .then(moods => {
        setAllMoods(moods.reduce((acc, mood) => ({
          ...acc,
          [mood.dateYYYYMMDD]: mood
        }), {}));
      })
  }, [])

  function setMood(moodLevel) {
    const mood = {
      dateYYYYMMDD: dateParam,
      type: 1,
      moodLevel,
    };
    fetch('http://localhost:7071/api/moods', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(mood)
    }).then(() => {
      setAllMoods({
        ...allMoods,
        [mood.dateYYYYMMDD]: mood
      });
    });
  }

  return (
    <>
      <Calendar
        onChange={setSelectedDate}
        value={selectedDate}
        tileContent={({ date }) => calendarTileContent(date, allMoods)} />
      <h2>{selectedDate.toDateString()}</h2>
      {allMoods && (
        <>
          <p>What's your mood today?</p>
          {Object.keys(moodLevels).map(moodLevel => (
            <button key={moodLevel} type="button" onClick={() => setMood(moodLevel)}>{moodLevels[moodLevel].emoji}</button>
          ))}
        </>
      )}
    </>
  );
}

function App() {
  return (
    <div className="app">
      <h1>Mooday</h1>
      <Router>
        <Switch>
          <Route path="/:dateParam" children={<Main />} />
          <Redirect to={dateToKey(new Date())} />
        </Switch>
      </Router>
    </div>
  );
}

export default App;
