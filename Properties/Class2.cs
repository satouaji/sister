﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1.Properties
{
    //★★スクリプトを書く際の注意★★////////////////////////////////
    ////○ラベルを切る行には他のことを書いてはいけない。
    //////////////////////////////////////////////////////////////////

    //★スクリプトの文法ルール////////////////////////////////////////
    ////○ラベル　　If文やJMP文で飛んでくる先。
    ////            　サンプル：
    ////            [L_***_***]:
    ////        ※注意！！ラベルを切る行には他のことを書いてはいけない。
    ////    　　
    ////○If文　　　( )内の条件を満たしたらラベルにジャンプ。
    ////　　　　　　　サンプル１（即値）：
    ////　　　　　　If ( A >= 30 ) [L_***_***]:
    ////         　　↑↑↑ ↑ ↑↑
    ////        　　これらの箇所では半角スペースを置くこと。
    ////            　サンプル２（変数）：
    ////            If ( A >= B ) [L_***_***]:
    ////            ・変数A,Bの型はint型に限る。
    ////
    ////○JMP文 　　指定したラベルにジャンプ。
    ////　　　　　　　サンプル：
    ////        　　JMP [L_***_***]:
    ////        　　  ↑
    ////        　　ここに半角スペースを置くこと。
    ////
    ////○計算　　　変数に値を代入したり、変数の値を変えたりする。
    ////　　　　　　　サンプル１（代入）：
    ////            計算 A <- B:
    ////　　　　　　　サンプル２（計算）：
    ////            計算 A += B:
    ////            計算 A -= B:
    ////            計算 A *= B:
    ////            計算 A /= B:
    ////　　　　　　　サンプル３（二項以上の計算）：
    ////            計算 A = B + C:
    ////            ・どの計算の場合も、右辺の項B,Cは、
    ////            　int型でさえあれば、即値でも変数でも良い。
    ////            ・演算子や等号の両側には半角スペースを置くこと。
    ////            ・割り算の場合、得られる結果は「商,あまり」のうちの商のみ。
    ////
    ////○コメント　"//"でその行のそれ以降の部分は無視される。
    //////////////////////////////////////////////////////////////////

    public class SE
    {

        /* ------------------ */
        /*      定数定義      */
        /* ------------------ */

        /* キャラ立ち絵番号 */
        public const int D_CHR_SARA_00 = 0;
        public const int D_CHR_SARA_01 = 1;
        public const int D_CHR_SARA_02 = 2;
        public const int D_CHR_SARA_03 = 3;
        public const int D_CHR_SARA_04 = 4;
        public const int D_CHR_SARA_05 = 5;
        public const int D_CHR_SARA_06 = 6;
        public const int D_CHR_SARA_07 = 7;
        public const int D_CHR_SARA_08 = 8;
        public const int D_CHR_SARA_09 = 9;
        public const int D_CHR_SARA_10 = 10;
        public const int D_CHR_SARA_11 = 11;
        public const int D_CHR_SARA_12 = 12;
        public const int D_CHR_SARA_13 = 13;
        public const int D_CHR_SARA_14 = 14;
        public const int D_CHR_SARA_15 = 15;
        public const int D_CHR_SARA_16 = 16;
        public const int D_CHR_SARA_17 = 17;
        public const int D_CHR_SARA_18 = 18;
        public const int D_CHR_SARA_19 = 19;
        public const int D_CHR_DEVIL_00 = 20;
        public const int D_CHR_DEVIL_01 = 21;
        public const int D_CHR_DEVIL_02 = 22;
        public const int D_CHR_DEVIL_03 = 23;
        public const int D_CHR_DEVIL_04 = 24;
        public const int D_CHR_DEVIL_05 = 25;

        /* パラメーター */

        int count = 0;              /* テキストファイル全体の中でのカウンタ */
        int countold = 0;           /* 前回処理終了時点でのカウンタの値 */
        int inrowcount = 0;         /* 一度に文章バッファに取り込む文章内でのカウンタ */

        static string[] log = new string[101];
        static string[] name = new string[101];

        static Parameter A_REG;           /* スクリプト上での計算時に値を取っておくのに使うための変数 */
        static Parameter B_REG;           /* スクリプト上での計算時に値を取っておくのに使うための変数 */
        static Parameter C_REG;           /* スクリプト上での計算時に値を取っておくのに使うための変数 */

        string text = Properties.Resources.opening;    /* ファイルの中身を文字の配列として取得 */


        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        /* ■　関数名：s_ScriptEngine　　　　　　 　　　　　　　 　■ */
        /* ■　入力：sentence_ct　文章の番号を示すカウンタ 　　　　■ */
        /* ■　　　　o_bgpic　　　背景画像のオブジェクト　 　　　　■ */
        /* ■　　　　o_charbox1 　左側キャラ画像のオブジェクト 　　■ */
        /* ■　　　　o_charbox2 　右側キャラ画像のオブジェクト 　　■ */
        /* ■　出力：sentence_ct　次回読み込み用のカウンタの値 　　■ */
        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */

        public int s_ScriptEngine(int sentence_ct, int log_ct, int log_ct_use, PictureBox o_bgpic, PictureBox o_chrbox1, PictureBox o_chrbox2, Sister Sis)
        {
            int i;
            const long D_CHAR_LAST = 100000;              /* １ファイルの最大文字数のDefine( ENDコードが無かった時の Fail Safe ) */
            const short D_WORKVAL_MAX = 100;


            //フォントオブジェクトの作成
            Font fnt = new Font("メイリオ", 12);

            Brush Color = Brushes.White;

            string textlawbuf;                            /* 文章バッファ */

            count = s_nowsenthead(sentence_ct);   /* テキスト内の初期値を取得 */
            countold = count;

            for (i = log_ct_use + 1; i <= 100; i++)
            {
                log[i] = " ";
                name[i] = " ";
            }

            while (count < D_CHAR_LAST)
            {
                if (this.text[count] == '/' && text[count + 1] == '/')
                {

                    /*====================*/
                    /*   コメントアウト   */
                    /*====================*/
                    while (checkrowlast(count) == 0)
                    {
                        count++;
                    }
                    count += 2;
                    countold = count;
                    inrowcount = 0;
                }
                else if (text[count] == ':')
                {
                    textlawbuf = text.Substring(countold, inrowcount);
                    
                    /*====================*/
                    /*    文法コマンド    */
                    /*====================*/
                    if (textlawbuf.Length >= 3 && textlawbuf.Substring(0, 3) == "end")
                    {
                        /*** 文章表示終了処理 ***/
                        sentence_ct = 0;
                        return sentence_ct;
                    }
                    else if (textlawbuf.Substring(0, 1) == "[")
                    {
                        /*** ラベル ***/
                        /* ラベルはすっ飛ばして次へ */

                        count++;

                        ////ここにcount += 2 を入れるべきかどうか////
                        count += 2;     /* 改行（ラベルの行には、何もコメントを書かないこと） */

                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                    }
                    else if (textlawbuf.Length >= 3 && textlawbuf.Substring(0, 3) == "JMP")
                    {
                        /*** ジャンプ ***/
                        count = text.IndexOf("\r\n" + textlawbuf.Remove(0, 4)) + 2;
                        countold = count;
                        sentence_ct = s_getnowsent(count);
                        inrowcount = 0;
                    }
                    else if (textlawbuf.Length >= 2 && textlawbuf.Substring(0, 2) == "計算")
                    {
                        /*** 計算 ***/

                        inrowcount = 3;
                        int inrowcountold = inrowcount;
                        int work_ct = 0;
                        Parameter work_1 = new Parameter();
                        int work_2 = 0;
                        Parameter right_1 = new Parameter();
                        Parameter right_2 = new Parameter();
                        int work_value_1 = 0;
                        int work_value_2 = 0;
                        bool int_flag_1 = false;            /*即値フラグ*/
                        bool int_flag_2 = false;            /*即値フラグ*/

                        /** 計算式左辺取得 **/
                        while (textlawbuf.Substring(inrowcount, 1) != " ")
                        {
                            inrowcount++;
                            work_ct++;
                        }
                        if (work_ct >= 2 && "体力" == textlawbuf.Substring(inrowcountold, work_ct))
                        {
                            work_1 = Sis.HitPoint;
                        }
                        else if (work_ct >= 2 && "気力" == textlawbuf.Substring(inrowcountold, work_ct))
                        {
                            work_1 = Sis.EnergyPoint;
                        }
                        else if (work_ct >= 3 && "性欲値" == textlawbuf.Substring(inrowcountold, work_ct))
                        {
                            work_1 = Sis.PassionPoint;
                        }
                        else if (work_ct >= 3 && "堕落度" == textlawbuf.Substring(inrowcountold, work_ct))
                        {
                            work_1 = Sis.CorruptionPoint;
                        }
                        else if (work_ct >= 3 && "汎用Ａ" == textlawbuf.Substring(inrowcountold, work_ct))
                        {
                            work_1 = A_REG;
                        }
                        else if (work_ct >= 3 && "汎用Ｂ" == textlawbuf.Substring(inrowcountold, work_ct))
                        {
                            work_1 = B_REG;
                        }
                        else if (work_ct >= 3 && "汎用Ｃ" == textlawbuf.Substring(inrowcountold, work_ct))
                        {
                            work_1 = C_REG;
                        }
                        else
                        {
                            Console.WriteLine("work_1 該当するパラメーターが存在しないようです");
                        }

                        inrowcount++;
                        inrowcountold = inrowcount;

                        /** 計算式等号取得 **/
                        while (textlawbuf.Substring(inrowcount, 1) != " ")
                        {
                            inrowcount++;
                            work_2++;
                        }

                        inrowcount++;
                        /* inrowcountoldは、演算子付き等号を後で拾うために動かさない */

                        if (2 >= work_2)
                        {
                            /* += -= *= /= <-(代入) */
                            /* 右辺の項は一つ */

                            /** 右辺取得 **/
                            if ("性欲値" == textlawbuf.Substring(inrowcount - work_ct))
                            {
                                right_1 = Sis.PassionPoint;
                            }
                            else if ("堕落度" == textlawbuf.Substring(inrowcount - work_ct))
                            {
                                right_1 = Sis.CorruptionPoint;
                            }
                            else
                            {
                                /* 整数値 */
                                while (work_value_1 < D_WORKVAL_MAX)
                                {
                                    if (textlawbuf.Substring(inrowcountold) == work_value_1.ToString())
                                    {
                                        break;
                                    }
                                    work_value_1++;
                                    int_flag_1 = true;
                                }  
                            }

                            if (int_flag_1 == true)
                            {
                                /* 右辺は即値 */
                                if ("+=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue += work_value_1;
                                }
                                else if ("-=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue -= work_value_1;
                                }
                                else if ("*=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue *= work_value_1;
                                }
                                else if ("/=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue /= work_value_1;
                                }
                                else if ("<-" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue = work_value_1;
                                }
                            }
                            else
                            {
                                /* 右辺は変数 */
                                if ("+=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue += right_1.CurrentValue;
                                }
                                else if ("-=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue -= right_1.CurrentValue;
                                }
                                else if ("*=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue *= right_1.CurrentValue;
                                }
                                else if ("/=" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue /= right_1.CurrentValue;
                                }
                                else if ("<-" == textlawbuf.Substring(inrowcountold, work_2))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue;
                                }
                            }
                        }
                        else
                        {
                            /* = */
                            /* 右辺の項は二つ */

                            inrowcountold = inrowcount;
                            /* こちらの分岐では演算子付き等号は出てこないので、inrowcountoldは更新してしまって良い */
                            
                            /** 右辺第一項取得 **/
                            while (textlawbuf.Substring(inrowcount, 1) != ":")
                            {
                                inrowcount++;
                                work_ct++;
                            }
                            if (work_ct >= 2 && "体力" == textlawbuf.Substring(inrowcount, work_ct))
                            {
                                work_1 = Sis.HitPoint;
                            }
                            else if (work_ct >= 2 && "気力" == textlawbuf.Substring(inrowcount, work_ct))
                            {
                                work_1 = Sis.EnergyPoint;
                            }
                            else if (work_ct >= 3 && "性欲値" == textlawbuf.Substring(inrowcount, work_ct))
                            {
                                right_1 = Sis.PassionPoint;
                            }
                            else if (work_ct >= 3 && "堕落度" == textlawbuf.Substring(inrowcount, work_ct))
                            {
                                right_1 = Sis.CorruptionPoint;
                            }
                            else
                            {
                                /* 整数値 */
                                while (work_value_1 < D_WORKVAL_MAX)
                                {
                                    if (textlawbuf.Substring(inrowcountold, work_ct) == work_value_1.ToString())
                                    {
                                        break;
                                    }
                                    work_value_1++;
                                    int_flag_1 = true;
                                }
                            }

                            inrowcount++;
                            inrowcountold = inrowcount;
                            /* こちらの分岐では演算子付き等号は出てこないので、inrowcountoldは更新してしまって良い */

                            /* 演算子の分カウンタを進める */
                            inrowcount++;
                            inrowcount++;
                            /* inrowcountoldは、演算子を後で拾うために動かさない */

                            /** 右辺第二項取得 **/
                            if ("性欲値" == textlawbuf.Substring(inrowcount))
                            {
                                right_2 = Sis.PassionPoint;
                            }
                            else if ("堕落度" == textlawbuf.Substring(inrowcount))
                            {
                                right_2 = Sis.CorruptionPoint;
                            }
                            else
                            {
                                /* 整数値 */
                                while (work_value_2 < D_WORKVAL_MAX)
                                {
                                    if (textlawbuf.Substring(inrowcount) == work_value_2.ToString())
                                    {
                                        break;
                                    }
                                    work_value_2++;
                                    int_flag_2 = true;
                                }
                            }

                            /** 計算 **/
                            if (int_flag_1 == true && int_flag_2 == true)
                            {
                                /* 右辺の両項とも即値 */
                                if ("+" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 + work_value_2;
                                }
                                else if ("-" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 - work_value_2;
                                }
                                else if ("*" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 * work_value_2;
                                }
                                else if ("/" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 / work_value_2;
                                }
                            }
                            else if (int_flag_1 == true && int_flag_2 == false)
                            {
                                /* 右辺の第一項：即値　第二項：変数 */
                                if ("+" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 + right_2.CurrentValue;
                                }
                                else if ("-" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 - right_2.CurrentValue;
                                }
                                else if ("*" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 * right_2.CurrentValue;
                                }
                                else if ("/" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = work_value_1 / right_2.CurrentValue;
                                }
                            }
                            else if (int_flag_1 == false && int_flag_2 == true)
                            {
                                /* 右辺の第一項：即値　第二項：変数 */
                                if ("+" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue + work_value_2;
                                }
                                else if ("-" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue - work_value_2;
                                }
                                else if ("*" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue * work_value_2;
                                }
                                else if ("/" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue / work_value_2;
                                }
                            }
                            else
                            {
                                /* 右辺の両項とも変数 */
                                if ("+" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue + right_2.CurrentValue;
                                }
                                else if ("-" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue - right_2.CurrentValue;
                                }
                                else if ("*" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue * right_2.CurrentValue;
                                }
                                else if ("/" == textlawbuf.Substring(inrowcountold, 1))
                                {
                                    work_1.CurrentValue = right_1.CurrentValue / right_2.CurrentValue;
                                }
                            }

                        }

                        /* 改行 */
                        count++;
                        count++;

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        inrowcountold = 0;

                    }
                    else if (textlawbuf.Length >= 3 && textlawbuf.Substring(0, 3) == "If(")
                    {
                        /*** 条件分岐 ***/

                        inrowcount = 4;
                        int inrowcountold = inrowcount;
                        int work_ct_1 = 0;
                        int work_ct_2 = 0;
                        Parameter work_1 = new Parameter();
                        int work_2 = 0;
                        int work_4 = 0;
                        Parameter work_5 = new Parameter();
                        int work_value = 0;
                        bool int_flag = false;

                        /** 条件左辺取得 **/
                        while (textlawbuf.Substring(inrowcount, 1) != " ")
                        {
                            inrowcount++;
                            work_ct_1++;
                        }
                        if (work_ct_1 >= 3 && "性欲値" == textlawbuf.Substring(inrowcountold, work_ct_1))
                        {
                            work_1 = Sis.PassionPoint;
                        }
                        else if (work_ct_1 >= 3 && "堕落度" == textlawbuf.Substring(inrowcountold, work_ct_1))
                        {
                            work_1 = Sis.CorruptionPoint;
                        }
                        else
                        {
                            Console.WriteLine("work_1 該当するパラメーターが存在しないようです");
                        }

                        inrowcount++;
                        inrowcountold = inrowcount;

                        /** 比較演算子取得 **/
                        while (textlawbuf.Substring(inrowcount, 1) != " ")
                        {
                            inrowcount++;
                            work_2++;
                        }

                        inrowcount++;
                        inrowcountold = inrowcount;

                        /** 条件右辺取得 **/
                        /* 文字数取得 */
                        while (textlawbuf.Substring(inrowcountold + work_ct_2, 1) != " ")
                        {
                            inrowcount++;
                            work_ct_2++;
                        }
                        if (work_ct_2 >= 2 && "体力" == textlawbuf.Substring(inrowcountold, work_ct_2))
                        {
                            work_5 = Sis.HitPoint;
                            int_flag = false;
                        }
                        else if (work_ct_2 >= 2 && "気力" == textlawbuf.Substring(inrowcountold, work_ct_2))
                        {
                            work_5 = Sis.EnergyPoint;
                            int_flag = false;
                        }
                        else if (work_ct_2 >= 3 && "性欲値" == textlawbuf.Substring(inrowcountold, work_ct_2))
                        {
                            work_5 = Sis.PassionPoint;
                            int_flag = false;
                        }
                        else if (work_ct_2 >= 3 && "堕落度" == textlawbuf.Substring(inrowcountold, work_ct_2))
                        {
                            work_5 = Sis.CorruptionPoint;
                            int_flag = false;
                        }
                        else if (textlawbuf.Length >= inrowcountold + 4 && textlawbuf.Substring(inrowcountold, work_ct_2) == "true")
                        {
                            /* true (bool) */
                            /* bool値は、true⇒1　false⇒0 と変換して使用する */

                            work_value = 1;
                            int_flag = true;
                        }
                        else if (textlawbuf.Length >= inrowcountold + 5 && textlawbuf.Substring(inrowcountold, work_ct_2) == "false")
                        {
                            /* false (bool) */

                            work_value = 0;
                            int_flag = true;
                        }
                        else
                        {
                            /* 整数値 */
                            while (work_value < D_WORKVAL_MAX)
                            {
                                if (textlawbuf.Substring(inrowcountold, work_ct_2) == work_value.ToString())
                                {
                                    break;
                                }
                                work_value++;
                            }
                            int_flag = true;
                        }

                        inrowcount += 3;

                        /** ジャンプ先ラベル文字数取得 **/
                        while (textlawbuf.Substring(inrowcount + work_4, 1) != "]")
                        {
                            work_4++;
                        }
                        work_4++;

                        /** 条件式に従ってジャンプ **/

                        if (int_flag == false)
                        {
                            if ("<" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {
                                if (work_1.CurrentValue < work_5.CurrentValue)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if ("<=" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue <= work_5.CurrentValue)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if (">" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue > work_5.CurrentValue)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if (">=" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue >= work_5.CurrentValue)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if ("==" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue == work_5.CurrentValue)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if ("!=" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue != work_5.CurrentValue)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }

                        }
                        else
                        {
                            int_flag = false;

                            if ("<" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {
                                if (work_1.CurrentValue < work_value)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if ("<=" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue <= work_value)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if (">" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue > work_value)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if (">=" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue >= work_value)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if ("==" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue == work_value)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }
                            else if ("!=" == textlawbuf.Substring(5 + work_ct_1, work_2))
                            {

                                if (work_1.CurrentValue != work_value)
                                {
                                    string aiueo = "\r\n" + textlawbuf.Substring(inrowcount, work_4);
                                    count = text.IndexOf(aiueo) + 2;
                                }
                                else
                                {
                                    /* 改行 */
                                    count++;
                                    count++;

                                    count++;
                                }
                            }

                        }
                        countold = count;
                        sentence_ct = s_getnowsent(count);
                        inrowcount = 0;
                        inrowcountold = 0;
                    }

                    /*======================*/
                    /*    発言者コマンド    */
                    /*======================*/
                    else if (textlawbuf == "Text")
                    {
                        Color = Brushes.White;

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        name[0] = "Text";
                    }
                    else if (textlawbuf == "サラ")
                    {
                        Color = Brushes.Pink;
                        s_disptachie(o_chrbox1, D_CHR_SARA_00);

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        name[0] = "サラ";

                    }
                    else if (textlawbuf == "マリー")
                    {
                        Color = Brushes.Yellow;

                        //    s_disptachie(o_chrbox1, D_CHR_SARA_00);

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        name[0] = "マリー";
                    }
                    else if (textlawbuf == "リディ")
                    {
                        Color = Brushes.Orange;

                        //    s_disptachie(o_chrbox1, D_CHR_SARA_00);

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        name[0] = "リディ";
                    }
                    else if (textlawbuf == "魔物")
                    {
                        Color = Brushes.Purple;

                        //    s_disptachie(o_chrbox1, D_CHR_SARA_00);

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        name[0] = "魔物";
                    }
                    else if (textlawbuf == "Plus")
                    {
                        Color = Brushes.Blue;

                        //    s_disptachie(o_chrbox1, D_CHR_SARA_00);

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        name[0] = "Plus";
                    }
                    else if (textlawbuf == "Minus")
                    {
                        Color = Brushes.Red;

                        //    s_disptachie(o_chrbox1, D_CHR_SARA_00);

                        count++;
                        countold = count;
                        sentence_ct++;
                        inrowcount = 0;
                        name[0] = "Minus";
                    }
                }
                else if (text[count] == ';')
                {

                    //描画先とするImageオブジェクトを作成する
                    Bitmap canvas = new Bitmap(o_bgpic.Width, o_bgpic.Height);
                    //ImageオブジェクトのGraphicsオブジェクトを作成する
                    Graphics g = Graphics.FromImage(canvas);

                    textlawbuf = text.Substring(countold, inrowcount);

                    g.DrawString(textlawbuf, fnt, Color, 0, 0);
                    //PictureBox1に表示する
                    o_bgpic.Image = canvas;

                    //リソースを解放する
                    fnt.Dispose();
                    g.Dispose();

                    count ++;
                    countold = count;
                    sentence_ct++;
                    inrowcount = 0;
                    log[0] = textlawbuf;
                    s_BacklogRenew(log_ct_use);
                    break;
                }
                else if ( count >= 2 && checkrowlast(count) == 1 && checkrowlast(count - 2) == 1 )
                {
                    /* 空白行 */
                    count += 2;
                    countold = count;
                    inrowcount = 0;
                }
                else
                {
                    count++;
                    inrowcount++;
                }
            }
            return sentence_ct;
        }



        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        /* ■　関数名：s_ScrollInit  　　　　　　 　　　　　　　 　■ */
        /* ■　内容：バックログ初期表示処理                     　 ■ */
        /* ■　入力：                                        　 　 ■ */
        /* ■　出力：                                        　 　 ■ */
        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        public void s_ScrollInit(int log_ct_use, PictureBox o_bgpic_0, PictureBox o_bgpic_1, PictureBox o_bgpic_2, PictureBox o_bgpic_3, PictureBox o_bgpic_4)
        {
            //フォントオブジェクトの作成
            Font fnt = new Font("メイリオ", 12);

            int i;

            Brush Color = Brushes.White;

            for (i = 1; i <= log_ct_use; i++)
            {
                if (name[i] == "Text")
                {
                    Color = Brushes.White;
                }
                else if (name[i] == "サラ")
                {
                    Color = Brushes.Pink;
                }
                else if (name[i] == "マリー")
                {
                    Color = Brushes.Yellow;
                }
                else if (name[i] == "リディ")
                {
                    Color = Brushes.Orange;
                }
                else if (name[i] == "魔物")
                {
                    Color = Brushes.Purple;
                }
                else if (name[i] == "Plus")
                {
                    Color = Brushes.Blue;
                }
                else if (name[i] == "Minus")
                {
                    Color = Brushes.Red;
                }

                if (i == 1)
                {
                    /* 描画先とするImageオブジェクトを作成する */
                    Bitmap canvas0 = new Bitmap(o_bgpic_0.Width, o_bgpic_0.Height);
                    /* ImageオブジェクトのGraphicsオブジェクトを作成する */
                    Graphics g0 = Graphics.FromImage(canvas0);
                    /* 描画内容を準備 */
                    g0.DrawString(log[i], fnt, Color, 0, 0);
                    /* PictureBoxに表示*/
                    o_bgpic_0.Image = canvas0;
                    /* リソースを解放 */
                    g0.Dispose();
                }
                else if (i == 2)
                {
                    Bitmap canvas1 = new Bitmap(o_bgpic_1.Width, o_bgpic_1.Height);
                    Graphics g1 = Graphics.FromImage(canvas1);
                    g1.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_1.Image = canvas1;
                    g1.Dispose();
                }
                else if (i == 3)
                {
                    Bitmap canvas2 = new Bitmap(o_bgpic_2.Width, o_bgpic_2.Height);
                    Graphics g2 = Graphics.FromImage(canvas2);
                    g2.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_2.Image = canvas2;
                    g2.Dispose();
                }
                else if (i == 4)
                {
                    Bitmap canvas3 = new Bitmap(o_bgpic_3.Width, o_bgpic_3.Height);
                    Graphics g3 = Graphics.FromImage(canvas3);
                    g3.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_3.Image = canvas3;
                    g3.Dispose();
                }
                else if (i == 5)
                {
                    Bitmap canvas4 = new Bitmap(o_bgpic_4.Width, o_bgpic_4.Height);
                    Graphics g4 = Graphics.FromImage(canvas4);
                    g4.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_4.Image = canvas4;
                    g4.Dispose();
                }

                if (i >= 5)
                {
                    break;
                }
            }
            //リソースを解放する
            fnt.Dispose();
        }


        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        /* ■　関数名：s_ScrollRedraw　　　　　　 　　　　　　　 　■ */
        /* ■　内容：バックログ初期表示処理                     　 ■ */
        /* ■　入力：                                        　 　 ■ */
        /* ■　出力：                                        　 　 ■ */
        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        public void s_ScrollRedraw(int log_ct, int log_ct_use, PictureBox o_bgpic_0, PictureBox o_bgpic_1, PictureBox o_bgpic_2, PictureBox o_bgpic_3, PictureBox o_bgpic_4)
        {
            //フォントオブジェクトの作成
            Font fnt = new Font("メイリオ", 12);

            int i;


            Brush Color = Brushes.White;

            for (i = log_ct; i <= log_ct_use; i++)
            {
                if (name[i] == "Text")
                {
                    Color = Brushes.White;
                }
                else if (name[i] == "サラ")
                {
                    Color = Brushes.Pink;
                }
                else if (name[i] == "マリー")
                {
                    Color = Brushes.Yellow;
                }
                else if (name[i] == "リディ")
                {
                    Color = Brushes.Orange;
                }
                else if (name[i] == "魔物")
                {
                    Color = Brushes.Purple;
                }
                else if (name[i] == "Plus")
                {
                    Color = Brushes.Blue;
                }
                else if (name[i] == "Minus")
                {
                    Color = Brushes.Red;
                }

                if (i == log_ct)
                {
                    /* 描画先とするImageオブジェクトを作成する */
                    Bitmap canvas0 = new Bitmap(o_bgpic_0.Width, o_bgpic_0.Height);
                    /* ImageオブジェクトのGraphicsオブジェクトを作成する */
                    Graphics g0 = Graphics.FromImage(canvas0);
                    /* 描画内容を準備 */
                    g0.DrawString(log[i], fnt, Color, 0, 0);
                    /* PictureBoxに表示*/
                    o_bgpic_0.Image = canvas0;
                    /* リソースを解放 */
                    g0.Dispose();
                }
                else if (i == log_ct + 1)
                {
                    Bitmap canvas1 = new Bitmap(o_bgpic_1.Width, o_bgpic_1.Height);
                    Graphics g1 = Graphics.FromImage(canvas1);
                    g1.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_1.Image = canvas1;
                    g1.Dispose();
                }
                else if (i == log_ct + 2)
                {
                    Bitmap canvas2 = new Bitmap(o_bgpic_2.Width, o_bgpic_2.Height);
                    Graphics g2 = Graphics.FromImage(canvas2);
                    g2.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_2.Image = canvas2;
                    g2.Dispose();
                }
                else if (i == log_ct + 3)
                {
                    Bitmap canvas3 = new Bitmap(o_bgpic_3.Width, o_bgpic_3.Height);
                    Graphics g3 = Graphics.FromImage(canvas3);
                    g3.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_3.Image = canvas3;
                    g3.Dispose();
                }
                else if (i == log_ct + 4)
                {
                    Bitmap canvas4 = new Bitmap(o_bgpic_4.Width, o_bgpic_4.Height);
                    Graphics g4 = Graphics.FromImage(canvas4);
                    g4.DrawString(log[i], fnt, Color, 0, 0);
                    o_bgpic_4.Image = canvas4;
                    g4.Dispose();
                }

                if (i >= log_ct + 5)
                {
                    break;
                }
            }
            //リソースを解放する
            fnt.Dispose();
        }


        /*////////////////////////ここからサブルーチン的メソッド////////////////////////*/


        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        /* ■　関数名：s_nowsenthead 　　　　　　 　　　　　　　 　■ */
        /* ■　内容：テキストの現在地取得処理                   　 ■ */
        /* ■　      sentence_ctからファイル中の位置を算出する  　 ■ */
        /* ■　　　　文章の送り時や、ロード後の最初の文探しに使用  ■ */
        /* ■　入力：sentence_ct                             　 　 ■ */
        /* ■　出力：                                        　 　 ■ */
        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */

        public int s_nowsenthead(int ct)
        {
            int i = 0;
            int k = 0;
            bool mark_active = false;
            int j;

            for (j = 0; j < ct; j++)
            {
                /* ;が来るまで開始位置からのカウンタを進める */
                while( i != -1 )
                {
                    if (text[i] == ';' || text[i] == ':')
                    {
                        /* ;や:が登場した場合、*/
                        /* コメント内部だったら一文字としてカウント */
                        /* コメント外だったら文章のカウンタをインクリメント */

                        k = i;
                        while ( text[k] != '/' || text[k+1] != '/' )
                        {
                            if ( text[k] == '\r' && text[k+1] == '\n' )
                            {
                                /* 前回のコメント記号より前に改行 ⇒ 使用中の;や: */
                                mark_active = true;
                                break;
                            }
                            else
                            {
                                k--;
                                if ( k <= 0 )
                                {
                                    mark_active = false;
                                    break;
                                }
                            }
                        }
                        if (mark_active == true)
                        {
                            /* 使用中の;や: */
                            mark_active = false;

                            /* ;や:出現後、次の文字が改行コードだったらさらに二文字進める */
                            if (text[i + 1] == '\r' && text[i + 2] == '\n')
                            {
                                i++;
                                i++;
                            }
                            break;
                        }
                        else
                        {
                            /* コメント内 */
                            i++;
                        }
                    }
                    else
                    {
                        /* ;や:以外の文字だったら文章のカウンタをインクリメント */
                        i++;
                    }
                }
            }
            i++;
            return i;
        }


        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        /* ■　関数名：s_getnowsent  　　　　　　 　　　　　　　 　■ */
        /* ■　内容：現在のファイル中の位置から     　             ■ */
        /* ■　　　  sentence_ctの値を算出する処理  　             ■ */
        /* ■　　　　ifやjmpなどでcountが急に変わった後に呼ぶ   　 ■ */
        /* ■　入力：sentence_ct                             　 　 ■ */
        /* ■　出力：                                        　 　 ■ */
        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */

        public int s_getnowsent(int ct)
        {
            int i = 0;
            int j;
            int k = 0;
            bool mark_active = false;

            for (j = 0; j < (ct); j++)
            {
//                if ( text[j] == ':' || text[j] == ';' )
//                {
//                    i++;
//                }
                if ( text[j] == ':' || text[j] == ';' )
                {
                    k = j;
                    while (text[k] != '/' || text[k + 1] != '/')
                    {
                        if (text[k] == '\r' && text[k + 1] == '\n')
                        {
                            /* 前回のコメント記号より前に改行 ⇒ 使用中の;や: */
                            mark_active = true;
                            break;
                        }
                        else
                        {
                            k--;
                            if (k <= 0)
                            {
                                mark_active = false;
                                break;
                            }
                        }
                    }
                    if (mark_active == true)
                    {
                        /* 使用中の;や: */
                        mark_active = false;
                        i++;
                    }
                    else
                    {
                        /* コメント内 */
                    }

                }
            }
            
                return i;
        }



        public int checkrowlast(int ct)
        {
            /* 改行判定処理 */
            /* コメントアウトの終端を判定するのに用いる */

            int i = 0;

            if (text[ct] == '\r' && text[ct + 1] == '\n')
            {
                i = 1;
            }

            return i;
        }

        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        /* ■　関数名：s_disptachie  　　　　　　 　　　　　　　 　■ */
        /* ■　内容：立ち絵を表示                            　 　 ■ */
        /* ■　入力：                                        　 　 ■ */
        /* ■　出力：                                        　 　 ■ */
        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */

        public void s_disptachie(PictureBox chrbox, int chrnum)
        {
            switch (chrnum)
            {
                case D_CHR_SARA_00:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_01:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_02:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_03:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_04:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_05:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_06:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_07:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_08:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_09:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_10:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;
                case D_CHR_SARA_11:
                    chrbox.BackgroundImage = Properties.Resources.sara_0_0;
                    break;

            }
            chrbox.Visible = true;
        }


        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */
        /* ■　関数名：s_BacklogRenew　　　　　　 　　　　　　　 　■ */
        /* ■　内容：バックログ用文字列配列を更新する処理    　 　 ■ */
        /* ■　入力：log_ct                                  　 　 ■ */
        /* ■　出力：log_ct                                  　 　 ■ */
        /* ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■ */

        public void s_BacklogRenew(int log_ct_use)
        {
            int i;
            string test_1;
            string test_2;

            for (i = log_ct_use + 1; i > 0; i--)
            {
                if (name[i].Length < name[i - 1].Length)
                {
                    name[i] = name[i].PadRight(name[i - 1].Length);
                }
                else if (name[i].Length > name[i - 1].Length)
                {
                    name[i] = name[i].Substring(0, name[i - 1].Length);
                }
                name[i] = name[i].Replace(name[i], name[i - 1]);


                if (log[i].Length < log[i - 1].Length)
                {
                    log[i] = log[i].PadRight(log[i - 1].Length);
                }
                else if (log[i].Length > log[i - 1].Length)
                {
                    log[i] = log[i].Substring(0, log[i - 1].Length);
                }
                string work_str = string.Copy(log[i]);
                log[i] = work_str.Replace(work_str, log[i - 1]);
                test_2 = log[i - 1];
                test_1 = log[i];
                test_2 = log[i - 1];

            }

            test_1 = log[0];
            test_2 = log[1];
            var test_3 = log[2];
            var test_4 = log[3];

            //return log_ct_use;
        }
    }
}

