import { Link } from 'react-router-dom';
import './Styles/Style.css';
import Icon from '../../VcsComponents/UtilComponents/Icons';

export default function Footer() {
    return (
        <footer className="text-white">
            <div className='col justify-content-center d-flex'>
                <ul className="bg-default rounded footer-list-item-inline footer-list">
                    <li className="list-inline-item">
                        <button data-bs-toggle="modal" data-bs-target="#AddItemModal" className="btn home-btn">
                            <Icon type="addFile" />
                        </button>
                    </li>
                    <li className="list-inline-item">
                        <button className="btn home-btn " data-bs-toggle="offcanvas" data-bs-target="#sidebar" aria-controls="sidebar">
                            <Icon type="user" />
                        </button>
                    </li>
                    <li className="list-inline-item">
                        <a href="" className="btn home-btn ">
                            <Icon type="bell" />
                        </a>
                    </li>
                    <li className="list-inline-item">
                        <Link to="/" className="btn home-btn">
                            <Icon type="home" />
                        </Link>
                    </li>
                    <li className="list-inline-item">
                        <Link to="/Repositories" className="btn home-btn">
                            <Icon type="stack" />
                        </Link>
                    </li>
                    <li className="list-inline-item">
                        <button className="btn home-btn">
                            <Icon type="favourite" />
                        </button>
                    </li>
                    <li className="list-inline-item">
                        <button className="btn home-btn" type="button" data-bs-toggle="offcanvas" data-bs-target="#vcs-sidebar" aria-controls="offcanvasExample">
                            <Icon type="leftSidebarSwitch" />
                        </button>
                    </li>
                </ul >
            </div>
        </footer>
    );
}