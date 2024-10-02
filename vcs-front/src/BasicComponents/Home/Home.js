import { Link } from 'react-router-dom';
import './Styles/Style.css'
import MarkdownBlock from '../../VcsComponents/UtilComponents/MarkdownBlock';

export default function () {

    const featuresInDevelopment = `
#### Upcoming features: 
- Backend Stats Dashboard
- Auth
- Merge Feature
- File Directory Support
- Pull Request Feature
- Commit History
- Syntax Highlighter
    `;

    return (
        <div className="container" id='centerELem'>
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
            <div className='row'>
                <div className='col'>
                    <div className=' card bg-default p-2'>
                        <div className='card-body bg-dark text-start'>
                            <MarkdownBlock classes={'text-light'} content={featuresInDevelopment} />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}