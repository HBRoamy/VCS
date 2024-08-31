import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import rehypeRaw from 'rehype-raw';

export default function MarkdownBlock({ content, classes }) {
    return (
        <div className={classes}>
            <ReactMarkdown 
                remarkPlugins={[remarkGfm]} 
                rehypePlugins={[rehypeRaw]}
            >
                {content ?? "## Nothing to display :("}
            </ReactMarkdown>
        </div>
    );
}
