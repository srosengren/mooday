import "./app.css";
import Calendar from "react-calendar";
import {
  BrowserRouter as Router,
  Switch,
  Redirect,
  Route,
  useParams,
  useHistory
} from "react-router-dom";

function dateToKey(date) {
  return date.getFullYear()+(`0${date.getMonth()+1}`.slice(-2))+date.getDate();
}

function Main() {
  const { dateParam } = useParams();
  const selectedDate = new Date(`${dateParam.substring(0,4)}-${dateParam.substring(4,6)}-${dateParam.substring(6,8)}`);
  const history = useHistory();

  function setSelectedDate(date) {
    history.push(dateToKey(date));
  }

  return (
    <>
      <Calendar onChange={setSelectedDate} value={selectedDate} />
      <h2>{selectedDate.toDateString()}</h2>
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
