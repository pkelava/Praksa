import React, { Component } from 'react';

class UkloniKupca extends Component {
    constructor(props){
        super(props);

        this.state = {
            SviKupci:[]
        };
    }
    
    componentDidMount(){
        fetch("https://localhost:44348/api/SviKupci").then(res => res.json()).then(
            result => {this.setState({SviKupci:result});
            }
        )
    }

    handleClick = (KupacID) => {
        let link = 'https://localhost:44348/api/UkloniKupca/' + KupacID;
        console.log(link);
        fetch(link , {
            method : 'DELETE'})
    }
    
    render() { 
        return ( 
            this.state.SviKupci.map( kupac => (
                <ul>
                    <li key={kupac.KupacID}>id: {kupac.KupacID}  ime: {kupac.Ime} prezime: {kupac.Prezime} <button onClick={() => this.handleClick(kupac.KupacID)}>Ukloni</button> </li>
                </ul>
            ))
         );
    }
}
 
export default UkloniKupca;