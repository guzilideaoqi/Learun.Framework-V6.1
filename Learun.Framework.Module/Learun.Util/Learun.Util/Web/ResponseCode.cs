namespace Learun.Util
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.03.08
    /// 描 述：接口响应码
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        success = 200,
        /// <summary>
        /// 失败
        /// </summary>
        fail = 400,
        /// <summary>
        /// 异常
        /// </summary>
        exception = 500,
        /// <summary>
        /// 用户未授权
        /// </summary>
        NoRelation = 101,
        /// <summary>
        /// 未登录
        /// </summary>
        NoLogin = 401,
        /// <summary>
        /// 登录失效
        /// </summary>
        LoginExpire = 402,
        /// <summary>
        /// 账户余额不足
        /// </summary>
        NoMoney = 102,
        /// <summary>
        /// 未绑定支付宝
        /// </summary>
        NoBindAliPay = 103,
        /// <summary>
        /// 未实名
        /// </summary>
        NoRealName = 104,
        /// <summary>
        /// 不存在的用户
        /// </summary>
        NoExistUser = 105
    }
}
