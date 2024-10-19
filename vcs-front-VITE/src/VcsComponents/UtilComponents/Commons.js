// Function to format the date as "Day MonthAbbr, Year"
export const formatDate = (dateString) => {
    // Assuming dateString is in "DD-MM-YYYY" format
    const [day, month, year] = dateString.split('-');  // Split the date into day, month, year
    const formattedDateString = `${year}-${month}-${day}`;  // Rearrange into "YYYY-MM-DD"

    const date = new Date(formattedDateString);  // Create a new Date object in the recognized format

    return date.toLocaleDateString('en-IN', {
        day: 'numeric',
        month: 'short',  // Abbreviated month (e.g., Jan, Feb)
        year: 'numeric'
    });
};
