import './App.css';
import { BrowserRouter as Router, Route, Routes, Navigate  } from 'react-router-dom';
import Footer from './BasicComponents/Footer/Footer';
import Header from './BasicComponents/Header/Header';
import Home from './BasicComponents/Home/Home';
import NotFound from './BasicComponents/NotFound/NotFoundPage';
import RepositoryForm from './VcsComponents/RepositoryComponent/RepositoryForm';
import RepositoryList from './VcsComponents/RepositoryComponent/RepositoryList';
import RepositoryDetail from './VcsComponents/RepositoryComponent/RepositoryDetail';
import RepositoryDashboard from './VcsComponents/RepositoryComponent/RepositoryDashboard';
import UserInfo from './VcsComponents/UserComponent/UserInfo';
import DiffComponent from './VcsComponents/PullComponent/DiffComponent';
import RepoBranchTree from './VcsComponents/RepositoryComponent/RepoBranchTree';
import CommitHistory from './VcsComponents/PullComponent/CommitHistory';
import CommitView from './VcsComponents/PullComponent/CommitView';
import Sandbox from './VcsComponents/UtilComponents/sandbox';

function App() {
  return (
    <div className="App">
      <div className="d-flex flex-column min-vh-100">
        <Header />

        <main className="flex-grow-1">
          <div className="container" style={{ marginBottom: '60px' }}>
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/Repositories/New" element={<RepositoryForm />} />
              <Route path="/Repositories" element={<RepositoryList />} />
              <Route path="/Repositories/:repoName" element={<RepositoryDetail />} />
              <Route path="/Repositories/:repoName/timeline" element={<RepositoryDashboard />} />
              <Route path="/About" element={<UserInfo />} />
              <Route path="/Compare/:repoName/:branchName" element={<DiffComponent />} />
              <Route path="/Repositories/:repoName/BranchTree" element={<RepoBranchTree />} />
              <Route path="/Repositories/:repoName/:branchName/CommitHistory" element={<CommitHistory />} />
              <Route path="/Repositories/:repoName/:branchName/:commitHash" element={<CommitView />} />
              <Route path="/Sandbox" element={<Sandbox />} />
              <Route path="/NotFound" element={<NotFound />} />
              <Route path="*" element={<NotFound />} />
            </Routes>
          </div>
        </main>

        <Footer />
      </div>
    </div>
  );
}

export default App;