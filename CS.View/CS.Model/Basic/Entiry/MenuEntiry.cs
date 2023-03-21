namespace CS.Model.Entiry
{
    public class MenuEntiry
    {
        /// <summary>
        ///
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        ///  菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        ///  菜单图标
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        ///  菜单实例
        /// </summary>
        public string MenuInstance { get; set; }

        /// <summary>
        ///  父菜单ID
        /// </summary>
        public long ParentID { get; set; }
    }
}