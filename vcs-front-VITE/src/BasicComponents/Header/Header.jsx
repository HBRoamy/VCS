import { Link } from 'react-router-dom';
import './Styles/Style.css';
import Icon from '../../VcsComponents/UtilComponents/Icons';

export default function Header() {
    return (
        <div>
            <header className="Main-bg text-white p-3">
                <button className="btn pulsate" type="button" data-bs-toggle="offcanvas" data-bs-target="#vcs-sidebar" aria-controls="offcanvasExample">
                    <h1 className="h3 text-white font-montserrat">
                        <Icon type="stack" classes='me-1 mb-1' />
                        <span className='Text-Color-Transition'>VCS</span></h1>
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