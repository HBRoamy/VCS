import React, { useState, useEffect } from 'react';
import { getRepositoryByName, saveRepoReadMe } from '../../Services/RepoService';
import DescriptionBlock from './DescriptionBlock';

export default function Sandbox() {
  const [repoName, setRepoName] = useState('sharadrepo');
  const [readMe, setReadMe] = useState('');

  useEffect(() => {
    const InitializeReadme = async () => {
      try {
        var getResponse = await getRepositoryByName(repoName)
        setReadMe(getResponse?.readMeBody)
      }
      catch (error) {
        console.error('Error fetching the readme:', error);
      }
      finally {
      }
    };

    InitializeReadme();
  }, [repoName]); // Dependency array to refetch if repoName changes

  const HandleUpdateContent = async (content) => {
    var updatedReadme = await saveRepoReadMe(repoName, content)
    setReadMe(updatedReadme)
  };

  return (
    <div>
      <DescriptionBlock content={readMe} onUpdate={HandleUpdateContent} />
    </div>
  );
}
