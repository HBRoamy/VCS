import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import rehypeRaw from 'rehype-raw';
import DOMPurify from 'dompurify';

export default function MarkdownBlock({ content, classes }) {
    return (
        <div className={classes}>
            <ReactMarkdown 
                remarkPlugins={[remarkGfm]} 
                rehypePlugins={[rehypeRaw]}
                children={DOMPurify.sanitize(content,{
                    //ALLOWED_TAGS: ['b', 'i', 'p', 'strong', 'em'], // Add allowed tags here
                    //ALLOWED_ATTR: [], // Disallow all attributes
                    FORBID_ATTR: ['data-toggle', 'data-target', 'onclick', 'onerror', 'onchange'],
                    FORBID_TAGS: ['script', 'iframe', 'button', 'input'] // Add tags you want to forbid
                }) ?? "### Add a readMe. HTML and Bootstrap supported."}
            />
        </div>
    );
}
