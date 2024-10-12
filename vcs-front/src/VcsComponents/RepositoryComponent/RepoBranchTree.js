import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Tree from 'react-d3-tree';
import { getRepositoryBranchTree } from '../../Services/RepoService';
import './Styles/TreeStyles.css';

function RepoBranchTree() {
    const { repoName } = useParams();
    const [treeData, setTreeData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [canZoom, setCanZoom] = useState(true);
    const [canDrag, setCanDrag] = useState(true);
    const [isVertical, setIsVertical] = useState(true);
    const [zoomChecked, setZoomChecked] = useState(true);
    const [dragChecked, setDragChecked] = useState(true);
    const [verticalChecked, setVerticalChecked] = useState(true);
    const [treeStyle, setTreeStyle] = useState('diagonal');

    useEffect(() => {
        const fetchHistory = async () => {
            try {
                setLoading(true);
                const data = await getRepositoryBranchTree(repoName);
                setTreeData(data);
                console.warn(data)
                setError(null); // Clear any previous errors
            } catch (error) {
                setError('Failed to fetch tree');
                console.error('Error fetching tree:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();
    }, [repoName]);

    const handleStyleChange = (style) => {
        setTreeStyle(style)
    }

    // Handlers for checkboxes
    const handleZoomChange = (e) => {
        const isChecked = e.target.checked;
        setZoomChecked(isChecked);
        setCanZoom(isChecked);
    };

    const handleDragChange = (e) => {
        const isChecked = e.target.checked;
        setDragChecked(isChecked);
        setCanDrag(isChecked);
    };

    const handleOrientationChange = (e) => {
        const isChecked = e.target.checked;
        setVerticalChecked(isChecked);
        setIsVertical(isChecked);
    };

    if (loading) {
        return (
            <div>
                <p className="text-warning">
                    <div className="spinner-border text-warning" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                </p>
            </div>
        );
    }

    if (error) {
        return <p className="text-danger">{error}</p>;
    }

    if (!treeData) {
        return <div>Loading...</div>;
    }

    const nodeSize = {
        x: '200',
        y: '200',
    };

    const nodeSvgShape = () => {
        return (
            <span className='btn btn-sm rounded'>
                Node
            </span>
        );
    };

    return (
        <>
            <h3 className='text-light h3 font-montserrat'>{repoName} Branch Tree</h3>
            <div className='bg-light rounded'>
                <div className=''>
                    <span className="nav-item dropdown dropdown-center">
                        <button className="btn btn-sm text-bg-warning dropdown-toggle mb-2 float-end mt-1 me-1" data-bs-toggle="dropdown" aria-expanded="false">
                            {treeStyle}
                        </button>
                        <ul className="dropdown-menu dropdown-menu-dark p-0">
                            <li>
                                <button className="dropdown-item" onClick={() => handleStyleChange('diagonal')}>
                                    diagonal
                                </button>
                            </li>
                            <li>
                                <button className="dropdown-item" onClick={() => handleStyleChange('step')}>
                                    step
                                </button>
                            </li>
                            <li>
                                <button className="dropdown-item" onClick={() => handleStyleChange('straight')}>
                                    straight
                                </button>
                            </li>
                            <li>
                                <button className="dropdown-item" onClick={() => handleStyleChange('elbow')}>
                                    elbow
                                </button>
                            </li>
                        </ul>
                    </span>
                    <div className='rounded badge mt-2 w-auto fw-bold font-raleway'>
                        <div className="form-control text-start">
                            <label>
                                <input
                                    type="checkbox"
                                    checked={zoomChecked}
                                    onChange={handleZoomChange}
                                />
                                &nbsp;Enable Zoom
                            </label>
                            <br />
                            <label>
                                <input
                                    type="checkbox"
                                    checked={dragChecked}
                                    onChange={handleDragChange}
                                />
                                &nbsp;Enable Drag
                            </label>
                            <br />
                            <label>
                                <input
                                    type="checkbox"
                                    checked={verticalChecked}
                                    onChange={handleOrientationChange}
                                />
                                &nbsp;Vertical Orientation
                            </label>
                        </div>
                    </div>
                </div>
                <div style={{
                    width: '100%',
                    height: 'calc(100vh - 200px)', // Adjust height as needed based on your layout
                    maxWidth: '80em', // Optional max width for large screens
                    margin: '0 auto', // Centering the container
                    padding: '1em' // Optional padding
                }}>
                    <Tree
                        data={treeData}
                        orientation={isVertical ? "vertical" : "horizontal"}
                        zoomable={canZoom}
                        draggable={canDrag}
                        translate={{ x: 500, y: 100 }} // Adjust this to center the tree better
                        rootNodeClassName="node__root"
                        branchNodeClassName="node__branch"
                        leafNodeClassName="node__leaf"
                        pathFunc={treeStyle} // step, straight, elbow, diagonal
                        nodeSize={nodeSize}
                    />
                </div>
            </div>
        </>

    );
}

export default RepoBranchTree;