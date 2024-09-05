using System;
using System.Collections.Concurrent;
using Sean.Utility.Enums;
using Sean.Utility.Impls.MQ;

namespace Sean.Utility.Contracts
{
    public interface ISimpleLocalMQ : IDisposable
    {
        /// <summary>
        /// 消息队列名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 消息队列类型
        /// </summary>
        MQType Type { get; }

        bool IsStarted { get; }

        void Start();
        void Stop();
    }

    /// <summary>
    /// 基于 <see cref="ConcurrentQueue{T}"/> 实现的线程安全的本地消息队列
    /// <para>- 支持多线程下的生产者\消费者模式</para>
    /// <para>- 支持多生产者、多消费者</para>
    /// <para>- 暂不支持消息持久化</para>
    /// </summary>
    public interface ISimpleLocalMQ<T> : ISimpleLocalMQ
    {
        #region 生产者
        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <returns></returns>
        ISimpleLocalMQProducer<T> CreateProducer();
        /// <summary>
        /// 添加生产者
        /// </summary>
        /// <param name="producer"></param>
        void AddProducer(ISimpleLocalMQProducer<T> producer);
        /// <summary>
        /// 移除生产者
        /// </summary>
        /// <param name="producer"></param>
        void RemoveProducer(ISimpleLocalMQProducer<T> producer);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        void Send(T data);
        #endregion

        #region 消费者
        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <returns></returns>
        ISimpleLocalMQConsumer<T> CreateConsumer(EventHandler<MessageReceivedEventArgs<T>> messageReceived = null);
        /// <summary>
        /// 添加消费者
        /// </summary>
        /// <param name="consumer"></param>
        void AddConsumer(ISimpleLocalMQConsumer<T> consumer);
        /// <summary>
        /// 移除消费者
        /// </summary>
        /// <param name="consumer"></param>
        void RemoveConsumer(ISimpleLocalMQConsumer<T> consumer);
        #endregion
    }
}