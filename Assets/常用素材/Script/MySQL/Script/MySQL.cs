using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imdork.Mysql;
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

public class MySQL : MonoBehaviour
{
    public static MySQL instance;

    public static SqlHelper mySqlTools = new SqlHelper("127.0.0.1", "3306", "root", "169548", "user");

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //创建数据库类                 IP地址       端口    用户名   密码     数据库项目名称
        //var mySqlTools = new SqlHelper("127.0.0.1", "3306", "root", "169548", "user");
        //打开数据库
        mySqlTools.Open();



        //创建表方法              表名       字段名称                              字段类型
        //mySqlTools.CreateTable("SixArmiesFiredInUnison", new[] { "UID", "PlayerName", "HeadPortrait", "TotalKilling", "Scores", "Ranking" }, new[] { "tinytext", "tinytext", "tinytext", "bigint", "bigint", "bigint" });


        //  插入方法                表名         字段名                             插入数据
        //mySqlTools.InsertInto("SixArmiesFiredInUnison", new[] { "UID", "PlayerName", "TotalKilling", "Scores", "Ranking" }, new[] { "2", "秋风落叶", "900", "0" ,"2" });


        //  插入方法                表名         字段名                             插入数据
        // mySqlTools.InsertInto("userdata", new[] { "UID", "User", "Password" }, new[] { "52022", "ddxj1", "123456" });

        //查询方法
        //FindMysql(mySqlTools, "SixArmiesFiredInUnison", new[] { "UID", "PlayerName", "TotalKilling", "Scores", "Ranking" });


        // 查询方法并计算Ranking
        //mySqlTools.FindAndCalculateRank(mySqlTools, "SixArmiesFiredInUnison", new[] { "UID", "PlayerName", "TotalKilling", "Scores", "Ranking" });
        //查询方法
        //FindMysql(mySqlTools, "userdata", new[] { "UID", "User", "Password" });


        //更新方法 表名         更新字段名 判断符号         更新数据 查询条件字段        条件成立字段
        // mySqlTools.UpdateIntoSpecific("userdata", new[] { "User" }, new[] { "=" }, new[] { "ddxj1" }, new[] { "Password" }, new[] { "456789" });

        //  删除方法         表名          删除字段        判断条件     条件成立数据         
        //mySqlTools.Delete("userdata", new[] { "User" }, new[] { "=" }, new[] { "ddxj1" });

        // 从SqlHelper查询出来数据库 都会返回Dataset  DataSet类               字段名
        //       返回对象object     获取数据方法    
        // var GetValues = MysqlTools.GetValue(mySqlTools.Select("userdata"), "User");
        //print(GetValues);

        //查询方法                         表名        查询字段名         判断字段名       判断符号        条件成立数据
        // var ds = mySqlTools.SelectWhere("userdata", new[] { "UID" }, new[] { "User" }, new[] { "=" }, new[] { "ddxj1" });
        //查询方法                         表名         判断字段名       判断符号        条件成立数据
        //var ds = mySqlTools.SelectWhere("userdata", new[] { "User" }, new[] { "=" }, new[] { "ddxj1" });

        //SelectWhere方法会返回Dataset类对象， 声明ds变量接收如上图
        //MysqlTools 工具类使用GetValue方法负责接收DataSet对象 给字段名称返回对应数据
        //调用MysqlTools 工具类                 Dataset类对象  查询字段
        //object values = MysqlTools.GetValue(ds, "UID");
        //print(values); //最后打印15924

        // mySqlTools.DeleteContents("userdata"); 删除表中全部数据


        //关闭数据库
        mySqlTools.Close();

        
    }


    /// <summary>
    /// 查询表中数据   记得先调用Open()方法  用完此方法后直接Close()
    /// </summary>
    /// <param name="mySqlTools">Mysql框架类</param>
    /// <param name="tableName">表名</param>
    /// <param name="items">字段名称</param>
    void FindMysql(SqlHelper mySqlTools,string tableName,string[] items)
    {
        var ds = mySqlTools.Select(tableName, items);
        var pairs = MysqlTools.TableData(ds);
        DebugMysql(pairs);      
    }
    /// <summary>
    /// 打印查询数据库
    /// </summary>
    /// <param name="pairs"></param>
    public void DebugMysql(Dictionary<string,object>[] pairs)
    {
        for (int i = 0; i < pairs.Length; i++)
        {
            foreach (var table in pairs[i])
            {
                string tableList = string.Format("第{0}行，表字段名对应数据是 {1}", i + 1, table);
                print(tableList);
            }
        }  
    }

    public void SavePlayerData(string uniqueID, string playerName, string userProfile, int killCount, int score)
    {
        // 创建数据库连接对象
        string connectionString = "Server=127.0.0.1;Port=3306;User=root;Password=169548;Database=user;CharSet=utf8;";



        MySqlConnection connection = new MySqlConnection(connectionString);

        // 创建数据库连接
        var mySqlTools = new SqlHelper("127.0.0.1", "3306", "root", "169548", "user");
        // 打开 mySqlTools 连接
        mySqlTools.Open();

        // 查询数据库以检查是否存在具有特定UID的玩家数据
        string query = $"SELECT * FROM SixArmiesFiredInUnison WHERE UID = '{uniqueID}'";
        print("query"+query);
        var ds = mySqlTools.ExecuteQuery(query);
        connection.Open();
        if (ds.Tables[0].Rows.Count == 0)
        {
            print("直接插入新玩家信息");
            // 直接插入新的玩家信息
            string insertQuery = "INSERT INTO SixArmiesFiredInUnison (UID, PlayerName, HeadPortrait, TotalKilling, Scores,Ranking) " +
                       "VALUES (@UID, @PlayerName, @HeadPortrait, @TotalKilling, @Scores, @Ranking)";

            // 打开 connection 连接
     

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@UID", uniqueID);
                // 替换特殊符号为下划线
                string sanitizedPlayerName = Regex.Replace(playerName, @"[^\p{L}\p{N}]+", "_");
                cmd.Parameters.AddWithValue("@PlayerName", sanitizedPlayerName);
                cmd.Parameters.AddWithValue("@HeadPortrait", userProfile);
                cmd.Parameters.AddWithValue("@TotalKilling", killCount);
                cmd.Parameters.AddWithValue("@Scores", score);
                cmd.Parameters.AddWithValue("@Ranking", 0);
                cmd.ExecuteNonQuery();
                UpdateRankings(mySqlTools);
       
            }

            // 关闭 connection 连接
        
        }
        else
        {
            print("更新玩家数据");
            // 更新玩家数据
            string updateQuery = "UPDATE SixArmiesFiredInUnison " +
                       "SET PlayerName = @PlayerName, HeadPortrait = @HeadPortrait, TotalKilling = @TotalKilling, Scores = @Scores " +
                       "WHERE UID = @UID";

            using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
            {
                cmd.Parameters.AddWithValue("@UID", uniqueID);
                string sanitizedPlayerName = Regex.Replace(playerName, @"[^\p{L}\p{N}]+", "_");
                cmd.Parameters.AddWithValue("@PlayerName", sanitizedPlayerName);
                cmd.Parameters.AddWithValue("@HeadPortrait", userProfile);
                cmd.Parameters.AddWithValue("@TotalKilling", killCount);
                cmd.Parameters.AddWithValue("@Scores", score);
                cmd.ExecuteNonQuery();  
                UpdateRankings(mySqlTools);
            }
        }
        // 关闭 mySqlTools 连接
        mySqlTools.Close();
    }



    private void UpdateRankings(SqlHelper mySqlTools)
    {
        // 创建查询语句，按Scores降序排列
        string query = "SELECT UID, Scores FROM SixArmiesFiredInUnison ORDER BY Scores DESC";

        // 执行查询
        var ds = mySqlTools.ExecuteQuery(query);

        // 更新Ranking
        int rank = 1;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            var id = row["UID"].ToString();

            // 使用 UPDATE 查询将Ranking更新到数据库中
            string updateRankingQuery = $"UPDATE SixArmiesFiredInUnison SET Ranking = {rank} WHERE UID = '{id}'";
            mySqlTools.ExecuteNonQuery(updateRankingQuery);

            rank++;
        }
    }



    // 创建一个方法，用于获取玩家的当前TotalKilling数量
    public int GetCurrentTotalKills(SqlHelper mySqlTools, string uniqueID)
    {
        string query = $"SELECT TotalKilling FROM SixArmiesFiredInUnison WHERE UID = '{uniqueID}'";
        var ds = mySqlTools.ExecuteQuery(query);
        var pairs = MysqlTools.TableData(ds);

        if (pairs.Length > 0 && pairs[0].ContainsKey("TotalKilling"))
        {
            return Convert.ToInt32(pairs[0]["TotalKilling"]);
        }

        return 0; // 如果未找到玩家的TotalKilling数量，返回0
    }

    // 创建一个方法，用于获取玩家的当前总Scores
    public int GetCurrentTotalScore(SqlHelper mySqlTools, string uniqueID)
    {
        string query = $"SELECT Scores FROM SixArmiesFiredInUnison WHERE UID = '{uniqueID}'";
        var ds = mySqlTools.ExecuteQuery(query);
        var pairs = MysqlTools.TableData(ds);

        if (pairs.Length > 0 && pairs[0].ContainsKey("Scores"))
        {
            return Convert.ToInt32(pairs[0]["Scores"]);
        }

        return 0; // 如果未找到玩家的总Scores，返回0
    }
}
 