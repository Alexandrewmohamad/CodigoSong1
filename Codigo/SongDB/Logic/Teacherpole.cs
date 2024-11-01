using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Teacherpole.
    /// </summary>
    public class Teacherpole
    {

        #region Fields *****************************************************************

        private int teacherPoleId;
        private int teacherId;
        private int poleId;

        #endregion Fields


        #region Constructors ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Teacherpole()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="teacherPoleId">The id of the Teacherpole.</param>
        public Teacherpole(int teacherPoleId)
        {
            this.teacherPoleId = teacherPoleId;
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        public Teacherpole(
            int teacherPoleId, int teacherId, int poleId)
        {
            this.teacherPoleId = teacherPoleId;
            this.teacherId = teacherId;
            this.poleId = poleId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.teacherPoleId;}
            set{ this.teacherPoleId = value;}
        }

        public int TeacherPoleId
        {
            get{ return this.teacherPoleId;}
            set{ this.teacherPoleId = value;}
        }

        public int TeacherId
        {
            get{ return this.teacherId;}
            set{ this.teacherId = value;}
        }

        public int PoleId
        {
            get{ return this.poleId;}
            set{ this.poleId = value;}
        }

        #endregion Properties


        #region Methods ****************************************************************

        /// <summary>
        /// Save Teacherpole to database.
        /// </summary>
        /// <returns>The id of the saved Teacherpole.</returns>
        public int Save()
        {
            teacherPoleId = Mapper.TeacherpoleMapper.Save(null, this);
            return teacherPoleId;
        }

        /// <summary>
        /// Save Teacherpole to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Teacherpole.</returns>
        public int Save(MySqlTransaction trans)
        {
            teacherPoleId = Mapper.TeacherpoleMapper.Save(trans, this);
            return teacherPoleId;
        }

        /// <summary>
        /// Delete Teacherpole by id.
        /// </summary>
        /// <param name="id">The id of the selected Teacherpole.</param>
        /// <returns>
        /// True if selected Teacherpole was deleted.
        /// False if selected Teacherpole was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.TeacherpoleMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Teacherpole by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Teacherpole.</param>
        /// <returns>
        /// True if selected Teacherpole was deleted.
        /// False if selected Teacherpole was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.TeacherpoleMapper.Delete(trans, id);
        }

        /// <summary>
        /// Find all Teacherpole.
        /// </summary>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> Find()
        {
            return Mapper.TeacherpoleMapper.Find(null);
        }

        /// <summary>
        /// Find all Teacherpole with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> Find(MySqlTransaction trans)
        {
            return Mapper.TeacherpoleMapper.Find(trans);
        }

        /// <summary>
        /// Find Teacherpole by id.
        /// </summary>
        /// <param name="id">The id of the selected Teacherpole</param>
        /// <returns>
        /// The selected Teacherpole.
        /// Null if selected Teacherpole was not found.
        /// </returns>
        public static Teacherpole Find(int id)
        {
            return Mapper.TeacherpoleMapper.Find(null, id);
        }

        /// <summary>
        /// Find Teacherpole by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Teacherpole</param>
        /// <returns>
        /// The selected Teacherpole.
        /// Null if selected Teacherpole was not found.
        /// </returns>
        public static Teacherpole Find(MySqlTransaction trans, int id)
        {
            return Mapper.TeacherpoleMapper.Find(trans, id);
        }

        /// <summary>
        /// Find all Teacherpole by Pole.
        /// </summary>
        /// <param name="poleId">The id of the selected pole.</param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> FindByPole(int poleId)
        {
            return Mapper.TeacherpoleMapper.FindByPole(null, poleId);
        }

        /// <summary>
        /// Find all Teacherpole by Pole with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="poleId">The id of the selected pole.</param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> FindByPole(MySqlTransaction trans, int poleId)
        {
            return Mapper.TeacherpoleMapper.FindByPole(trans, poleId);
        }

        /// <summary>
        /// Find all Teacherpole by Teacher.
        /// </summary>
        /// <param name="teacherId">The id of the selected teacher.</param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> FindByTeacher(int teacherId)
        {
            return Mapper.TeacherpoleMapper.FindByTeacher(null, teacherId);
        }

        /// <summary>
        /// Find all Teacherpole by Teacher with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="teacherId">The id of the selected teacher.</param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> FindByTeacher(MySqlTransaction trans, int teacherId)
        {
            return Mapper.TeacherpoleMapper.FindByTeacher(trans, teacherId);
        }

        #endregion Methods

    } //end of class Teacherpole

} //end of namespace PnT.SongDB.Logic
