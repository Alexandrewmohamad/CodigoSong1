using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for TeacherpoleMapper.
    /// </summary>
    public class TeacherpoleMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Teacherpole to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Teacherpole.</returns>
        public static int Save(MySqlTransaction trans, Teacherpole teacherpole)
        {
            return Access.TeacherpoleAccess.Save(trans, GetParameters(teacherpole));
        }

        /// <summary>
        /// Delete Teacherpole by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Teacherpole.</param>
        /// <returns>
        /// True if selected Teacherpole was deleted.
        /// False if selected Teacherpole was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.TeacherpoleAccess.Delete(trans, id);
        }

        /// <summary>
        /// Find all Teacherpole.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.TeacherpoleAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Teacherpole by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Teacherpole.
        /// Null if selected Teacherpole was not found.
        /// </returns>
        public static Teacherpole Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.TeacherpoleAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Teacherpole by Pole.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="poleId">The id of the selected pole.</param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> FindByPole(MySqlTransaction trans, int poleId)
        {
            DataRow[] dr = Access.TeacherpoleAccess.FindByPole(trans, poleId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Teacherpole by Teacher.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="teacherId">The id of the selected teacher.</param>
        /// <returns>
        /// List of Teacherpole objects.
        /// Null if no Teacherpole was found.
        /// </returns>
        public static List<Teacherpole> FindByTeacher(MySqlTransaction trans, int teacherId)
        {
            DataRow[] dr = Access.TeacherpoleAccess.FindByTeacher(trans, teacherId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Teacherpole objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Teacherpole objects.</returns>
        private static List<Teacherpole> Map(DataRow[] rows)
        {
            List<Teacherpole> teacherpoles = new List<Teacherpole>();

            for (int i = 0; i < rows.Length; i++)
                teacherpoles.Add(Map(rows[i]));

            return teacherpoles;
        }

        /// <summary>
        /// Map database row to a Teacherpole object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Teacherpole</returns>
        private static Teacherpole Map(DataRow row)
        {
            Teacherpole teacherpole = new Teacherpole((int)(row["teacherPoleId"]));
            teacherpole.TeacherId = (int)DataAccessCommon.HandleDBNull(row,"teacherId", typeof(int));
            teacherpole.PoleId = (int)DataAccessCommon.HandleDBNull(row,"poleId", typeof(int));

            return teacherpole;
        }

        #endregion Mapper Methods

        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Teacherpole
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Teacherpole teacherpole)
        {
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("teacherPoleId",teacherpole.Id);
            parameters[1] = new MySqlParameter("teacherId",teacherpole.TeacherId);
            parameters[2] = new MySqlParameter("poleId",teacherpole.PoleId);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class TeacherpoleMapper

} //end of namespace PnT.SongDB.Mapper
