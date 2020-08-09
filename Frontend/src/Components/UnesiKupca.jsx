import React, { Component } from 'react';


function UnesiKupca({match}){
    return ( <Unosi proizvodid={match.params.id}/>);
}

class Unosi extends Component {
    constructor(props){
        super(props);
        this.state = {
            proizvodid : this.props.proizvodid,
            Ime : "",
            Prezime : "",
            NacinPlacanja : "",
        }
    }

    handleChangeFirstName = (event) => {
        this.setState({[event.target.name]: event.target.value});
    }

    handleChangeLastName = (event) => {
        this.setState({[event.target.name]: event.target.value});
    }

    handleChangePay = (event) => {
        this.setState({[event.target.name]: event.target.value});
    }

    handleSubmit = (event) => {
    let link = 'https://localhost:44348/api/NovaKupovina/'+ this.state.proizvodid;
    let kupac = {
        Ime : this.state.Ime,
        Prezime : this.state.Prezime,
        NacinPlacanja : this.state.NacinPlacanja,
    }

    fetch( link,{
        method: 'POST',
        headers:{'Content-type' : 'application/json'},
        body:JSON.stringify(kupac)
    }).then(r => r.json()).then(res =>{
        if(res){
            console.log(JSON.stringify(kupac));
        }
    })

    event.preventDefault();
    }

    render() { 
        return ( <div>
            Ispuni Podatke: {this.state.proizvodid} {this.state.ime}
            <form onSubmit={this.handleSubmit}>
                <p>
                    <label>
                        Ime: 
                        <input type="text" placeholder='Your First Name' name='Ime' onChange={this.handleChangeFirstName}/>
                    </label>
                </p>
                <p>
                    <label>
                        Prezime: 
                        <input type="text" placeholder='Your Last Name' name='Prezime' onChange={this.handleChangeLastName}/>
                    </label>
                </p>
                <p>
                    <label>
                        Nacin placanja: 
                        <input type="text" placeholder='Your choice of payement' name='NacinPlacanja' onChange={this.handleChangePay}/>
                    </label>
                </p>
                <p>
                    <input type="submit" value="Potvrdi" />
                </p>
            </form>
    
            
        </div>  );
    }
}
export default UnesiKupca;
