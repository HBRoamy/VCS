import React from 'react';
import ReactMarkdown from 'react-markdown';

const markdown = '# Hello, *world*!';

function Sandbox() {
  return (
    <div>
      <ReactMarkdown>{markdown}</ReactMarkdown>
    </div>
  );
}

export default App;
