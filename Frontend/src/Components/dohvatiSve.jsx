import React, { Component } from 'react';

class DohvatiSve extends Component {
    constructor(props){
        super(props);

        this.state = {
            dohvatiSve:[],
        };
    }
    
    componentDidMount(){
        fetch("https://localhost:44348/api/DohvatiSve").then(res => res.json()).then(
            result => {this.setState({dohvatiSve:result});
            }
        )
    }


    render() { 
        return (  
            <div>
                <h2>Infomracije o kupovinama...</h2>
                <table class="blueTable">
                    <thead>
                        <tr>
                            <th>id</th>
                            <th>Ime</th>
                            <th>Prezime</th>
                            <th>NacinPlacanja</th>
                            <th>DatumKupovine</th>
                            <th>ProizvodID</th>
                            <th>NazivProizvoda</th>
                            <th>CijenaProizvoda</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.dohvatiSve.map(emp => (
                            <tr key={emp.KupacID}>
                                <td>{emp.KupacID}</td>
                                <td>{emp.Ime}</td>
                                <td>{emp.Prezime}</td>
                                <td>{emp.NacinPlacanja}</td>
                                <td>{emp.DatumKupovine}</td>
                                <td>{emp.ProizvodID}</td>
                                <td>{emp.NazivProizvoda}</td>
                                <td>{emp.CijenaProizvoda} </td>
                                
                            </tr>
                        ))} 
                        
                    </tbody>
                </table>
            </div>
        );
    }
}
 
export default DohvatiSve;