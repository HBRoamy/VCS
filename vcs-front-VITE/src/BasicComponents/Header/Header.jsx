import { Route, Routes, Link } from 'react-router-dom';
import Home from '../Home/Home';
import RepositoryList from '../../VcsComponents/RepositoryComponent/RepositoryList';
import './Styles/Style.css';

export default function Header() {
    return (
        <div>
            <header className="Main-bg text-white p-3">
                <button className="btn pulsate" type="button" data-bs-toggle="offcanvas" data-bs-target="#vcs-sidebar" aria-controls="offcanvasExample">
                    <h1 className="h3 text-white font-montserrat"><svg xmlns="http://www.w3.org/2000/svg" width="16"
                        height="16" fill="currentColor" className="bi bi-stack" viewBox="0 0 16 16">
                        <path
                            d="m14.12 10.163 1.715.858c.22.11.22.424 0 .534L8.267 15.34a.598.598 0 0 1-.534 0L.165 11.555a.299.299 0 0 1 0-.534l1.716-.858 5.317 2.659c.505.252 1.1.252 1.604 0l5.317-2.66zM7.733.063a.598.598 0 0 1 .534 0l7.568 3.784a.3.3 0 0 1 0 .535L8.267 8.165a.598.598 0 0 1-.534 0L.165 4.382a.299.299 0 0 1 0-.535L7.733.063z" />
                        <path
                            d="m14.12 6.576 1.715.858c.22.11.22.424 0 .534l-7.568 3.784a.598.598 0 0 1-.534 0L.165 7.968a.299.299 0 0 1 0-.534l1.716-.858 5.317 2.659c.505.252 1.1.252 1.604 0l5.317-2.659z" />
                    </svg> <span className='Text-Color-Transition'>VCS</span></h1>
                </button>

            </header>

            <div className="offcanvas offcanvas-start" data-bs-theme="dark" tabindex="-1" id="vcs-sidebar" aria-labelledby="offcanvasLabel">
                <div className="offcanvas-header">
                    <h5 className="offcanvas-title" id="offcanvasLabel">VCS</h5>
                    <button type="button" className="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div className="offcanvas-body">
                    <div>
                        <div className="list-group text-start">
                            <Link to="/" className="list-group-item list-group-item-action" >Home</Link>
                            <Link to="/Repositories" className="list-group-item list-group-item-action">Repositories</Link>
                            <Link to="/Pulls" className="list-group-item list-group-item-action">Pull Requests</Link>
                            <Link to="/About" className="list-group-item list-group-item-action">About</Link>
                        </div>
                    </div>
                </div>
                <hr />
                <div className="dropdown">
                    <a href="#" className="d-flex align-items-center ms-2 mb-3 text-white text-decoration-none dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                        <img src="https://github.com/hbroamy.png" alt="" width="32" height="32" className="rounded-circle me-2" />
                        <strong>Harshit Bhardwaj</strong>
                    </a>
                    <ul className="dropdown-menu dropdown-menu-dark text-small shadow">
                        <li><Link to="/Repositories" className="dropdown-item">New Repository</Link></li>
                        <li><a className="dropdown-item" href="#">Settings</a></li>
                        <li><a className="dropdown-item" href="#">Profile</a></li>
                        <li><hr className="dropdown-divider" /></li>
                        <li><a className="dropdown-item text-danger" href="#">Sign out</a></li>
                    </ul>
                </div>
            </div>
        </div>
    );
}