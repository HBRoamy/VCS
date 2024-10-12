import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { dark } from 'react-syntax-highlighter/dist/esm/styles/prism';

const SyntaxHighlighterBlock = ({language, content}) => {
  return (
    <SyntaxHighlighter language={language} style={dark} customStyle={{
      padding: 0,
      border: 'none',
      background: 'transparent',
      boxShadow: 'none', // Removes any shadow
      outline: 'none'    // Removes any outlines
    }}>
      {content}
    </SyntaxHighlighter>
  );
};

export default SyntaxHighlighterBlock;