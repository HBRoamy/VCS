import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom'; // Import useParams from react-router-dom

export default function PullRequest() {
    const { repoName, pullNumber } = useParams();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
}