import './App.css';
import { BrowserRouter as Router, Route, Routes, Navigate  } from 'react-router-dom';
import Footer from './BasicComponents/Footer/Footer';
import Header from './BasicComponents/Header/Header';
import Home from './BasicComponents/Home/Home';
import NotFound from './BasicComponents/NotFound/NotFoundPage';
import RepositoryForm from './VcsComponents/RepositoryComponent/RepositoryForm';
import RepositoryList from './VcsComponents/RepositoryComponent/RepositoryList';
import RepositoryDetail from './VcsComponents/RepositoryComponent/RepositoryDetail';
import UserInfo from './VcsComponents/UserComponent/UserInfo';

function App() {
  return (
    <div className="App">
      <div className="d-flex flex-column min-vh-100">
        <Header />

        <main className="flex-grow-1">
          <div className="container">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/Repositories/New" element={<RepositoryForm />} />
              <Route path="/Repositories" element={<RepositoryList />} />
              <Route path="/Repositories/:repoName" element={<RepositoryDetail />} />
              <Route path="/About" element={<UserInfo />} />
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