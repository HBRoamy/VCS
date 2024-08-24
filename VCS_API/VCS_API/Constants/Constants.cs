namespace Constants
{
    public static class Constants
    {
        public const string StandardColumnDelimiter = ", ";
        public const string ItemAddressDelimiter = "#";//used to create the address of an item, present in a file, e.g. filepath#MyUniqueIdentifier or repoName#branchName
        public const string NullPlaceholder = "0/";
        public const string MasterBranchName = "Master";
        public const string RepositoryName = "{repoName}";
        public const string BaseBranchName = "{baseBranch}";
        public const string RepoAndBranchName = $"{Constants.RepositoryName}/Base/{Constants.BaseBranchName}";
        public const string RepoAndBranchCompare = "{repoName}/Compare/{branchName}";
    }
}
