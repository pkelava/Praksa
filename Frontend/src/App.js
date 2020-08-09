import React from 'react';
import './App.css';
import {BrowserRouter as Router , Switch, Route, Link } from 'react-router-dom';
import DohvatiSve from './Components/dohvatiSve.jsx'
import NovaKupovina from './Components/NovaKupovina.jsx';
import UnesiKupca from './Components/UnesiKupca.jsx';
import UkloniKupca from './Components/UkloniKupca.jsx';

function App() {

  return (
    <Router>
      <div className="App">
        <header className="App.header">
          <nav>

              <Link to="/DohvatiSve" class="gore">DohvatiSve</Link>

              <Link to="/Proizvod" class="gore">Proizvod</Link>

              <Link to="/UkloniKupca" class="gore">UkloniKupca</Link>

          </nav>
        {/* A <Switch> looks through its children <Route>s and
            renders the first one that matches the current URL. */}

        <Switch>
          
          <Route path="/DohvatiSve" component={DohvatiSve}/>
          <Route path="/Proizvod" component={NovaKupovina}/>
          <Route exact path="/UnesiKupca/:id" component={UnesiKupca}/>
          <Route path="/UkloniKupca" component={UkloniKupca}/>
        </Switch>
        </header>
      </div>
    </Router>
  );
}



export default App;
