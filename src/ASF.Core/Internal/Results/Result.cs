﻿using Newtonsoft.Json;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 结果对象
    /// </summary>
    public class Result
    {
        public Result() { }
        public Result(string message, int status )
        {
            this.Status = status;
            this.Message = message;
        }
        public Result(ValueTuple<int, string>  result)
        {
            this.Status = result.Item1;
            this.Message = result.Item2;
        }
        /// <summary>
        /// 执行是否成功
        /// </summary>
        [JsonIgnore]
        public bool Success
        {
            get
            {
                return this.Status == 200;
            }
        }
        /// <summary>
        /// 业务返回码
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        /// <summary>
        /// 执行返回消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(Result result)
        {
            this.Status = result.Status;
            this.Message = result.Message;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(string message, int status)
        {
            this.Status = status;
            this.Message = message;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result">结果对象</param>
        protected void To(ValueTuple<int,string> result)
        {
            this.Status = result.Item1;
            this.Message = result.Item2;
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public static Result ReFailure(string message, int status)
        {
            return new Result(message, status);
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public static Result ReFailure(ValueTuple<int, string> result)
        {
            return new Result(result.Item2, result.Item1);
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static T ReFailure<T>(Result result) where T : Result, new()
        {
            T r = new T();
            r.To(result);
            return r;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public static T ReFailure<T>(string message, int status) where T : Result, new()
        {
            T result = new T();
            result.To(message, status);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public static T ReFailure<T>(ValueTuple<int, string> result) where T : Result, new()
        {
            T ret = new T();
            ret.To(result);
            return ret;
        }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result ReSuccess()
        {
            return new Result(BaseResultCodes.Success);
        }
        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static T ReSuccess<T>() where T : Result, new()
        {
            T result = new T();
            result.To(BaseResultCodes.Success);
            return result;
        }
        /// <summary>
        /// 转换为 <see cref="Task<Result>"/>
        /// </summary>
        /// <returns></returns>
        public  Task<Result> AsTask()
        {
            return Task.FromResult(this);
        }
    }
    /// <summary>
    /// 实体结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 实体结果
        /// </summary>
        public Result() { }
        /// <summary>
        /// 实体结果
        /// </summary>
        /// <param name="data"></param>
        public Result(T data) : base(BaseResultCodes.Success)
        {
            this.Data = data;
        }
        public Result(T data, ValueTuple<int, string> result) : base(result)
        {
            this.Data = data;
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public T Data { get; set; }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result<T> ReSuccess(T data)
        {
            return new Result<T>(data);
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public new static Result<T> ReFailure(string message, int status)
        {
            Result<T> result = new Result<T>();
            result.To(message, status);
            return result;
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public new static Result<T> ReFailure(ValueTuple<int, string> result)
        {
            Result<T> res = new Result<T>();
            res.To(result);
            return res;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static Result<T> ReFailure(Result result) 
        {
            Result<T> re = new Result<T>();
            re.To(result);
            return re;
        }
        /// <summary>
        /// 转换为 <see cref="Task<Result<T>>"/>
        /// </summary>
        /// <returns></returns>
        public new Task<Result<T>> AsTask()
        {
            return Task.FromResult(this);
        }
    }
}