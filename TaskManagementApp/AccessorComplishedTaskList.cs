﻿//********************
//Designer:鈴木智仁
//Date:2021/12/23
//Purpose:完了したタスク情報をJSON形式で残す
//********************
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace TaskManagementApp
{
    public class AccessorComplishedTaskList 
    {
        /// <summary>
        /// 外部変数
        /// ID :1 
        /// タスクリスト
        /// </summary>
        static public List<Task> complishedTaskList;
        //デバッグ用のパス
        //string filePath = System.IO.Directory.GetCurrentDirectory() + @"\..\..\F1_TaskData\taskData.json";//@特殊な文字を文字としてそのまま適用する
        //リリース用のパス
        string filePath = System.IO.Directory.GetCurrentDirectory() + @"\compTaskData.json";
        /// <summary>
        /// このメソッドは、MainWindow.xaml.csで、始めに1回だけ呼ばれる
        /// </summary>
        public void InitializeJsonData()
        {
            //このプロジェクトの実行exeカレントディレクトリのパスを取得
            if (File.Exists(filePath))
            {
                string getJsonString = System.IO.File.ReadAllText(filePath);//jsonfileが存在するなら読み込む
                //JsonSerializerを使うためには、対象となるクラスのフィールドには、プロパティを設定していないといけない
                complishedTaskList = JsonSerializer.Deserialize<List<Task>>(getJsonString);//Jsonファイルからクラスリスト生成
            }
            else //jsonファイルが存在しないなら
            {
                List<Task> templist = new List<Task>();
                //Task tempTask = new Task() { taskID = 1, taskInfo = "taskInfo", taskSummary = "summary", taskLimit = DateTime.Now.ToString(), taskPriority = 1 };
                //templist.Add(tempTask);
                string tempJsonString = JsonSerializer.Serialize<List<Task>>(templist);
                System.IO.File.WriteAllText(filePath, tempJsonString);//新しくjsonファイルを生成
                InitializeJsonData();//再度読み直す　そういう意図での再帰呼び出し
            }

            ViewTaskListToCosole();
        }

        public void WriteJsonData()
        {

            string outJsonString = JsonSerializer.Serialize<List<Task>>(complishedTaskList);//クラスからJsonStringを生成
            System.IO.File.WriteAllText(filePath, outJsonString);//@特殊な文字を文字としてそのまま適用する
        }
        /// <summary>
        /// 取得したファイル情報から、タスクのリストを生成し渡す．
        /// </summary>
        /// <returns></returns>
        public List<Task> GetTaskList()
        {
            return complishedTaskList;

        }
        /// <summary>
        /// タスクをtasklistへ追加する、その後それをjsonデータとして書き出す
        /// </summary>
        /// <param name="addTask"></param>
        public void AddTaskList(Task addTask)
        {
            complishedTaskList.Add(addTask);
            WriteJsonData();
        }
        /// <summary>
        /// tasklistから指定したタスクを削除する、その後それをjsonデータとして書き出す
        /// </summary>
        /// <param name="addTask"></param>
        public void RemoveTaskList(Task removeTask)
        {
            complishedTaskList.Remove(removeTask);
            WriteJsonData();
        }
        /// <summary>
        /// 引数にとったタスクリストをコピー(値渡し)して返すメソッド
        /// </summary>
        /// <param name="inputTasks"></param>
        /// <returns></returns>
        static public List<Task> CopyTaskList(List<Task> inputTasks)
        {
            List<Task> resultTaskList = new List<Task>();
            foreach (Task task in inputTasks)
            {
                resultTaskList.Add(task);
            }
            return resultTaskList;
        }
        public void ViewTaskListToCosole()
        {
            foreach (Task task in complishedTaskList)
            {
                Console.WriteLine(task.taskID);
                Console.WriteLine(task.taskSummary);
                Console.WriteLine(task.taskInfo);
                Console.WriteLine(DateTime.Parse(task.taskLimit).ToString());
                Console.WriteLine(task.taskPriority);
            }
        }
    }
}