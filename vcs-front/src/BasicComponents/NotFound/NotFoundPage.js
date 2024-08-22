import React from 'react';
import { Link } from 'react-router-dom';
import './Styles/Style.css';

function NotFound() {
  return (
    <div className="text-center text-light">
      <h1 className='display-4 fw-bold Text-Color-Transition'>404</h1>
      <p className='fs-4 font-montserrat'>Oopsie daisy, page not found.</p>
      <h1 className='display-1 fw-bold'>¯\_(ツ)_/¯</h1>
      <Link to="/" className="btn btn-danger mt-4">Home</Link>
    </div>
  );
}

export default NotFound;