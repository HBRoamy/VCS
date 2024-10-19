import { Link } from 'react-router-dom';
import './Styles/Style.css'
import MarkdownBlock from '../../VcsComponents/UtilComponents/MarkdownBlock';

export default function () {

    //   <img src="/webpage_bg.jpg" class="card-img" alt="background">
    const featuresInDevelopment = `
<div class="card fw-bold bg-dark row">
<div class="col">
Upcoming features: 
<ul class="list-group">
  <li class="list-group-item">Backend Stats Dashboard</li>
  <li class="list-group-item">Auth</li>
  <li class="list-group-item">Merge Feature</li>
  <li class="list-group-item">Pull Request Feature</li>
</ul>
</div>
<div class="col">
NOTES: 
<ul class="list-group list-group-flush">
  <li class="list-group-item">Repo url should show branch name as well, if branch name is null, its master branch.</li>
  <li class="list-group-item">Links to commits should take us to a view with the diff of the commit and its parent commit.</li>
  <li class="list-group-item">A third item</li>
</ul>
</div>
</div>
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
            {
                false && <div className='row'>
                    <div className='col'>
                        <div className=' card bg-default p-2'>
                            <div className='card-body bg-dark text-start'>
                                <MarkdownBlock classes={'text-light'} content={featuresInDevelopment} />
                            </div>
                        </div>
                    </div>
                </div>
            }
        </div>
    );
}