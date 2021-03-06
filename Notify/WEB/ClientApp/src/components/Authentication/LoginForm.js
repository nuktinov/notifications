﻿import React, { useState } from 'react';
import { connect } from 'react-redux'
import './Authentication.css'


import { loginRequest } from '../../actions/authentication'
import  AuthenticationError  from './AuthenticationError'

function LoginForm({ authenticate}) {
    const [form, setValues] = useState({
        userName: "",
        password: ""
    });

    const updateField = e => {
        setValues({
            ...form,
            [e.target.name]: e.target.value
        });
    };

    const getToken = () => {
        authenticate(form);
    }

    return (
        <div >
            <div className="login-container" >
            <h1> Log In </h1>
            <form>
                <label>
                        <h4> Username: </h4>
                <input
                        value={form.userName}
                        name="userName"
                        onChange={updateField}
                    />
                </label>
                <br />
                <label>
                        <h4> Password:</h4>
                <input
                        value={form.password}
                        name="password"
                        type="password"
                        onChange={updateField}
                    />
                </label>
                <br />
                </form>
                <div className="form-button">
                    <button className="submit-button"
                        onClick={getToken}>
                            Submit
                    </button>
                </div>
            </div>
            <br />
            <AuthenticationError />
        </div>

    );
}



const mapDispatchToProps = dispatch => {
    return {
        authenticate: (user) => dispatch(loginRequest(user))
    }
}

export default connect(null, mapDispatchToProps)(LoginForm)
