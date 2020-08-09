import React, { Component } from 'react';
import {Link} from 'react-router-dom';

class NovaKupovina extends Component {

    render() { 
        return (
            <div>
                <ol>
                    <li>Biljeznica 5kn <Link to="/UnesiKupca/1">Kupi</Link></li>
                    
                    <li>Pribor za pisanje 30kn <Link to={{pathname : "/UnesiKupca/2", state : {proizvodid : 2}}}>Kupi</Link> </li>
                    <li>Gemoetrijski pribor 50kn <Link to="/UnesiKupca/3">Kupi</Link> </li>
                    <li>Kalkulator 180kn <Link to="/UnesiKupca/4">Kupi</Link> </li>
                    <li>Pernica 40kn <Link to="/UnesiKupca/5">Kupi</Link> </li>
                    <li>Torba 60kn <Link to="/UnesiKupca/6">Kupi</Link> </li>
                </ol>
            </div>
          );
    }
}
 
export default NovaKupovina;