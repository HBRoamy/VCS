import { Link } from 'react-router-dom';

export default function () {
    return (
        <div className="container mt-4">
            <div className="row justify-content-center">
                <div className="col-8">
                    <div className="h-100 p-4 text-light">
                        <h2 className='display-6 fw-bold font-montserrat'>Welcome to VCS</h2>
                        <figure className="text-center">
                            <blockquote className="blockquote fst-italic mt-4 Text-Color-Transition">
                                <p>It's not what you look at that matters, it's what you see.</p>
                            </blockquote>
                            <figcaption className="blockquote-footer">
                                Henry David Thoreau
                            </figcaption>
                        </figure>
                        <p className="font-montserrat mt-4">Create codebases, request code changes, review and merge pull requests. And a lot more useful features.</p>
                        <p className="font-montserrat">Click the VCS logo on top to explore further. Or get started by simply clicking the button below.</p>
                        <Link to={`/Repositories/New`} className="btn btn-success font-raleway">
                            Create a Repository
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}