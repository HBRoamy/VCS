// SvgIcons.js
import React from "react";

const Icons = {
  addFile: ({ height = null, width = null, classes = '' }) => {
    // Declare the classes variable outside JSX
    const combinedClasses = `bi bi-file-earmark-plus ${classes}`;

    return (
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill='currentColor'
        className={combinedClasses} // Use the combined className here
        viewBox="0 0 16 16"
      >
        <path d="M8 6.5a.5.5 0 0 1 .5.5v1.5H10a.5.5 0 0 1 0 1H8.5V11a.5.5 0 0 1-1 0V9.5H6a.5.5 0 0 1 0-1h1.5V7a.5.5 0 0 1 .5-.5z" />
        <path d="M14 4.5V14a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V2a2 2 0 0 1 2-2h5.5L14 4.5zm-3 0A1.5 1.5 0 0 1 9.5 3V1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V4.5h-2z" />
      </svg>
    );
  },
  user: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-person-circle ${classes}`;

    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z" />
        <path fill-rule="evenodd" d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z" />
      </svg>
    )
  },
  bell: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-bell ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2zM8 1.918l-.797.161A4.002 4.002 0 0 0 4 6c0 .628-.134 2.197-.459 3.742-.16.767-.376 1.566-.663 2.258h10.244c-.287-.692-.502-1.49-.663-2.258C12.134 8.197 12 6.628 12 6a4.002 4.002 0 0 0-3.203-3.92L8 1.917zM14.22 12c.223.447.481.801.78 1H1c.299-.199.557-.553.78-1C2.68 10.2 3 6.88 3 6c0-2.42 1.72-4.44 4.005-4.901a1 1 0 1 1 1.99 0A5.002 5.002 0 0 1 13 6c0 .88.32 4.2 1.22 6z" />
      </svg>
    )
  },
  home: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-house ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M8.707 1.5a1 1 0 0 0-1.414 0L.646 8.146a.5.5 0 0 0 .708.708L2 8.207V13.5A1.5 1.5 0 0 0 3.5 15h9a1.5 1.5 0 0 0 1.5-1.5V8.207l.646.647a.5.5 0 0 0 .708-.708L13 5.793V2.5a.5.5 0 0 0-.5-.5h-1a.5.5 0 0 0-.5.5v1.293L8.707 1.5ZM13 7.207V13.5a.5.5 0 0 1-.5.5h-9a.5.5 0 0 1-.5-.5V7.207l5-5 5 5Z" />
      </svg>
    )
  },
  stack: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-stack ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="m14.12 10.163 1.715.858c.22.11.22.424 0 .534L8.267 15.34a.598.598 0 0 1-.534 0L.165 11.555a.299.299 0 0 1 0-.534l1.716-.858 5.317 2.659c.505.252 1.1.252 1.604 0l5.317-2.66zM7.733.063a.598.598 0 0 1 .534 0l7.568 3.784a.3.3 0 0 1 0 .535L8.267 8.165a.598.598 0 0 1-.534 0L.165 4.382a.299.299 0 0 1 0-.535L7.733.063z" />
        <path d="m14.12 6.576 1.715.858c.22.11.22.424 0 .534l-7.568 3.784a.598.598 0 0 1-.534 0L.165 7.968a.299.299 0 0 1 0-.534l1.716-.858 5.317 2.659c.505.252 1.1.252 1.604 0l5.317-2.659z" />
      </svg>
    )
  },
  favourite: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-bookmark-star ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M7.84 4.1a.178.178 0 0 1 .32 0l.634 1.285a.178.178 0 0 0 .134.098l1.42.206c.145.021.204.2.098.303L9.42 6.993a.178.178 0 0 0-.051.158l.242 1.414a.178.178 0 0 1-.258.187l-1.27-.668a.178.178 0 0 0-.165 0l-1.27.668a.178.178 0 0 1-.257-.187l.242-1.414a.178.178 0 0 0-.05-.158l-1.03-1.001a.178.178 0 0 1 .098-.303l1.42-.206a.178.178 0 0 0 .134-.098L7.84 4.1z" />
        <path d="M2 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v13.5a.5.5 0 0 1-.777.416L8 13.101l-5.223 2.815A.5.5 0 0 1 2 15.5V2zm2-1a1 1 0 0 0-1 1v12.566l4.723-2.482a.5.5 0 0 1 .554 0L13 14.566V2a1 1 0 0 0-1-1H4z" />
      </svg>
    )
  },
  leftSidebarSwitch: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-layout-sidebar-inset ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M14 2a1 1 0 0 1 1 1v10a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V3a1 1 0 0 1 1-1zM2 1a2 2 0 0 0-2 2v10a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V3a2 2 0 0 0-2-2z" />
        <path d="M3 4a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v8a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1z" />
      </svg>
    )
  },
  copy: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-copy ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 14}
        height={height ?? 14}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 18 18">
        <path fillRule="evenodd" d="M4 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2zm2-1a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1zM2 5a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1v-1h1v1a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h1v1z" />
      </svg>
    )
  },
  history: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-clock-history ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 18}
        height={height ?? 18}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M8.515 1.019A7 7 0 0 0 8 1V0a8 8 0 0 1 .589.022zm2.004.45a7 7 0 0 0-.985-.299l.219-.976q.576.129 1.126.342zm1.37.71a7 7 0 0 0-.439-.27l.493-.87a8 8 0 0 1 .979.654l-.615.789a7 7 0 0 0-.418-.302zm1.834 1.79a7 7 0 0 0-.653-.796l.724-.69q.406.429.747.91zm.744 1.352a7 7 0 0 0-.214-.468l.893-.45a8 8 0 0 1 .45 1.088l-.95.313a7 7 0 0 0-.179-.483m.53 2.507a7 7 0 0 0-.1-1.025l.985-.17q.1.58.116 1.17zm-.131 1.538q.05-.254.081-.51l.993.123a8 8 0 0 1-.23 1.155l-.964-.267q.069-.247.12-.501m-.952 2.379q.276-.436.486-.908l.914.405q-.24.54-.555 1.038zm-.964 1.205q.183-.183.35-.378l.758.653a8 8 0 0 1-.401.432z" />
        <path d="M8 1a7 7 0 1 0 4.95 11.95l.707.707A8.001 8.001 0 1 1 8 0z" />
        <path d="M7.5 3a.5.5 0 0 1 .5.5v5.21l3.248 1.856a.5.5 0 0 1-.496.868l-3.5-2A.5.5 0 0 1 7 9V3.5a.5.5 0 0 1 .5-.5" />
      </svg>
    )
  },
  edit: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-pencil-square ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
        <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z" />
      </svg>
    )
  },
  textLeftAlign: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-text-left ${classes}`;

    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path fill-rule="evenodd" d="M2 12.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5m0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5m0-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5m0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5" />
      </svg>
    )
  },
  check: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-check2 ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 22}
        height={height ?? 22}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0z" />
      </svg>
    )
  },
  cross: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-x-lg ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
      </svg>
    )
  },
  fileDiff: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-file-diff ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 20}
        height={height ?? 19}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M8 4a.5.5 0 0 1 .5.5V6H10a.5.5 0 0 1 0 1H8.5v1.5a.5.5 0 0 1-1 0V7H6a.5.5 0 0 1 0-1h1.5V4.5A.5.5 0 0 1 8 4m-2.5 6.5A.5.5 0 0 1 6 10h4a.5.5 0 0 1 0 1H6a.5.5 0 0 1-.5-.5" />
        <path d="M2 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2zm10-1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1" />
      </svg>
    )
  },
  info: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-info-circle ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16" />
        <path d="m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0" />
      </svg>
    )
  },
  eyeSlash: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-eye-slash ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 18}
        height={height ?? 18}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M13.359 11.238C15.06 9.72 16 8 16 8s-3-5.5-8-5.5a7 7 0 0 0-2.79.588l.77.771A6 6 0 0 1 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755q-.247.248-.517.486z" />
        <path d="M11.297 9.176a3.5 3.5 0 0 0-4.474-4.474l.823.823a2.5 2.5 0 0 1 2.829 2.829zm-2.943 1.299.822.822a3.5 3.5 0 0 1-4.474-4.474l.823.823a2.5 2.5 0 0 0 2.829 2.829" />
        <path d="M3.35 5.47q-.27.24-.518.487A13 13 0 0 0 1.172 8l.195.288c.335.48.83 1.12 1.465 1.755C4.121 11.332 5.881 12.5 8 12.5c.716 0 1.39-.133 2.02-.36l.77.772A7 7 0 0 1 8 13.5C3 13.5 0 8 0 8s.939-1.721 2.641-3.238l.708.709zm10.296 8.884-12-12 .708-.708 12 12z" />
      </svg>
    )
  },
  eye: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-eye ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 18}
        height={height ?? 18}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8M1.173 8a13 13 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5s3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5s-3.879-1.168-5.168-2.457A13 13 0 0 1 1.172 8z" />
        <path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5M4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0" />
      </svg>
    )
  },
  link45: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-link-45deg ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M4.715 6.542 3.343 7.914a3 3 0 1 0 4.243 4.243l1.828-1.829A3 3 0 0 0 8.586 5.5L8 6.086a1 1 0 0 0-.154.199 2 2 0 0 1 .861 3.337L6.88 11.45a2 2 0 1 1-2.83-2.83l.793-.792a4 4 0 0 1-.128-1.287z" />
        <path d="M6.586 4.672A3 3 0 0 0 7.414 9.5l.775-.776a2 2 0 0 1-.896-3.346L9.12 3.55a2 2 0 1 1 2.83 2.83l-.793.792c.112.42.155.855.128 1.287l1.372-1.372a3 3 0 1 0-4.243-4.243z" />
      </svg>
    )
  },
  lock: ({ height = null, width = null, classes = '', title = 'lock' }) => {
    const combinedClasses = `bi bi-lock ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 24}
        height={height ?? 24}
        fill="currentColor"
        className={combinedClasses}
        title={title}
        viewBox="0 0 16 16">
        <path d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2m3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2M5 8h6a1 1 0 0 1 1 1v5a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V9a1 1 0 0 1 1-1" />
      </svg>
    )
  },
  unlock: ({ height = null, width = null, classes = '', title = 'unlock' }) => {
    const combinedClasses = `bi bi-unlock ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 24}
        height={height ?? 24}
        fill="currentColor"
        className={combinedClasses}
        title={title}
        viewBox="0 0 16 16">
        <path d="M11 1a2 2 0 0 0-2 2v4a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V9a2 2 0 0 1 2-2h5V3a3 3 0 0 1 6 0v4a.5.5 0 0 1-1 0V3a2 2 0 0 0-2-2M3 8a1 1 0 0 0-1 1v5a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V9a1 1 0 0 0-1-1z" />
      </svg>
    )
  },
  infoFill: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-info-circle-fill ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 20}
        height={height ?? 20}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16m.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2" />
      </svg>
    )
  },
  archive: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-archive ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 20}
        height={height ?? 20}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M0 2a1 1 0 0 1 1-1h14a1 1 0 0 1 1 1v2a1 1 0 0 1-1 1v7.5a2.5 2.5 0 0 1-2.5 2.5h-9A2.5 2.5 0 0 1 1 12.5V5a1 1 0 0 1-1-1zm2 3v7.5A1.5 1.5 0 0 0 3.5 14h9a1.5 1.5 0 0 0 1.5-1.5V5zm13-3H1v2h14zM5 7.5a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5" />
      </svg>
    )
  },
  card: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-card-text ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 20}
        height={height ?? 20}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M14.5 3a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-13a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5zm-13-1A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2z" />
        <path d="M3 5.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5M3 8a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9A.5.5 0 0 1 3 8m0 2.5a.5.5 0 0 1 .5-.5h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5" />
      </svg>
    )
  },
  codeJournal: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-journal-code ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 70}
        height={height ?? 70}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path fill-rule="evenodd" d="M8.646 5.646a.5.5 0 0 1 .708 0l2 2a.5.5 0 0 1 0 .708l-2 2a.5.5 0 0 1-.708-.708L10.293 8 8.646 6.354a.5.5 0 0 1 0-.708m-1.292 0a.5.5 0 0 0-.708 0l-2 2a.5.5 0 0 0 0 .708l2 2a.5.5 0 0 0 .708-.708L5.707 8l1.647-1.646a.5.5 0 0 0 0-.708" />
        <path d="M3 0h10a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2v-1h1v1a1 1 0 0 0 1 1h10a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H3a1 1 0 0 0-1 1v1H1V2a2 2 0 0 1 2-2" />
        <path d="M1 5v-.5a.5.5 0 0 1 1 0V5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0V8h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0v.5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1z" />
      </svg>
    )
  },
  star: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-star-fill ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 10}
        height={height ?? 10}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
      </svg>
    )
  },
  verified: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-patch-check ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path fill-rule="evenodd" d="M10.354 6.146a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L7 8.793l2.646-2.647a.5.5 0 0 1 .708 0z" />
        <path d="m10.273 2.513-.921-.944.715-.698.622.637.89-.011a2.89 2.89 0 0 1 2.924 2.924l-.01.89.636.622a2.89 2.89 0 0 1 0 4.134l-.637.622.011.89a2.89 2.89 0 0 1-2.924 2.924l-.89-.01-.622.636a2.89 2.89 0 0 1-4.134 0l-.622-.637-.89.011a2.89 2.89 0 0 1-2.924-2.924l.01-.89-.636-.622a2.89 2.89 0 0 1 0-4.134l.637-.622-.011-.89a2.89 2.89 0 0 1 2.924-2.924l.89.01.622-.636a2.89 2.89 0 0 1 4.134 0l-.715.698a1.89 1.89 0 0 0-2.704 0l-.92.944-1.32-.016a1.89 1.89 0 0 0-1.911 1.912l.016 1.318-.944.921a1.89 1.89 0 0 0 0 2.704l.944.92-.016 1.32a1.89 1.89 0 0 0 1.912 1.911l1.318-.016.921.944a1.89 1.89 0 0 0 2.704 0l.92-.944 1.32.016a1.89 1.89 0 0 0 1.911-1.912l-.016-1.318.944-.921a1.89 1.89 0 0 0 0-2.704l-.944-.92.016-1.32a1.89 1.89 0 0 0-1.912-1.911l-1.318.016z" />
      </svg>
    )
  },
  merge: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        fill="#fff"
        width={width ?? 20}
        height={height ?? 20}
        className={combinedClasses}
        viewBox="0 0 512 512">
        <path d="M385,224a64,64,0,0,0-55.33,31.89c-42.23-1.21-85.19-12.72-116.21-31.33-32.2-19.32-49.71-44-52.15-73.35a64,64,0,1,0-64.31.18V360.61a64,64,0,1,0,64,0V266.15c44.76,34,107.28,52.38,168.56,53.76A64,64,0,1,0,385,224ZM129,64A32,32,0,1,1,97,96,32,32,0,0,1,129,64Zm0,384a32,32,0,1,1,32-32A32,32,0,0,1,129,448ZM385,320a32,32,0,1,1,32-32A32,32,0,0,1,385,320Z" />
      </svg>
    )
  },
  plusMinus: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `bi bi-plus-slash-minus ${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        width={width ?? 16}
        height={height ?? 16}
        fill="currentColor"
        className={combinedClasses}
        viewBox="0 0 16 16">
        <path d="m1.854 14.854 13-13a.5.5 0 0 0-.708-.708l-13 13a.5.5 0 0 0 .708.708M4 1a.5.5 0 0 1 .5.5v2h2a.5.5 0 0 1 0 1h-2v2a.5.5 0 0 1-1 0v-2h-2a.5.5 0 0 1 0-1h2v-2A.5.5 0 0 1 4 1m5 11a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5A.5.5 0 0 1 9 12" />
      </svg>
    )
  },
  pushPull: ({ height = null, width = null, classes = '' }) => {
    const combinedClasses = `${classes}`;
    return (
      <svg xmlns="http://www.w3.org/2000/svg"
        fill="#fff"
        width={width ?? 20}
        height={height ?? 20}
        className={combinedClasses}
        viewBox="0 0 512 512" >
        <path d="M218.31,340.69A16,16,0,0,0,191,352v32H171a28,28,0,0,1-28-28V152a64,64,0,1,0-64-1.16V356a92.1,92.1,0,0,0,92,92h20v32a16,16,0,0,0,27.31,11.31l64-64a16,16,0,0,0,0-22.62ZM112,64A32,32,0,1,1,80,96,32,32,0,0,1,112,64Z" />
        <path d="M432,360.61V156a92.1,92.1,0,0,0-92-92H320V32a16,16,0,0,0-27.31-11.31l-64,64a16,16,0,0,0,0,22.62l64,64A16,16,0,0,0,320,160V128h20a28,28,0,0,1,28,28V360.61a64,64,0,1,0,64,0ZM400,448a32,32,0,1,1,32-32A32,32,0,0,1,400,448Z" />
      </svg>
    )
  },
  // Add more icons here
};


const Icon = ({ type, height = null, width = null, classes = '' }) => {
  const IconComponent = Icons[type];

  // If the icon type does not exist, return null or a fallback
  if (!IconComponent) return null;

  // Render the selected icon with passed props
  return <IconComponent height={height} width={width} classes={classes} />;
};

export default Icon;
