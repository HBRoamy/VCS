import React from 'react';
import Icon from './Icons';

function CopyButton({ text, title = 'copy' }) {
    return (
        <button className='btn btn-sm bg-transparent' title={title} onClick={() => navigator.clipboard.writeText(text)}>
            <Icon type="copy" classes="text-light mb-1" />
        </button>
    );
};

export default CopyButton;
