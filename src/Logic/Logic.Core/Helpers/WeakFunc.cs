namespace devdeer.DoctorFlox.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Interfaces;

    /// <summary>
    /// Stores a Func&lt;T&gt; without causing a hard reference
    /// to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result of the Func that will be stored
    /// by this weak reference.
    /// </typeparam>
    public class WeakFunc<TResult>
    {
        #region member vars

        private Func<TResult> _staticFunc;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="func">The Func that will be associated to this instance.</param>
        public WeakFunc(Func<TResult> func) : this(func?.Target, func)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="target">The Func's owner.</param>
        /// <param name="func">The Func that will be associated to this instance.</param>
        public WeakFunc(object target, Func<TResult> func)
        {
            if (func.Method.IsStatic)
            {
                _staticFunc = func;
                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }
                return;
            }
            Method = func.Method;
            FuncReference = new WeakReference(func.Target);
            Reference = new WeakReference(target);
        }

        /// <summary>
        /// Initializes an empty instance of the WeakFunc class.
        /// </summary>
        protected WeakFunc()
        {
        }

        #endregion

        #region methods

        /// <summary>
        /// Executes the action. This only happens if the Func's owner
        /// is still alive.
        /// </summary>
        /// <returns>The result of the Func stored as reference.</returns>
        public TResult Execute()
        {
            if (_staticFunc != null)
            {
                return _staticFunc();
            }
            var funcTarget = FuncTarget;
            if (IsAlive)
            {
                if (Method != null && FuncReference != null && funcTarget != null)
                {
                    return (TResult)Method.Invoke(funcTarget, null);
                }
            }
            return default(TResult);
        }

        /// <summary>
        /// Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            Reference = null;
            FuncReference = null;
            Method = null;
            _staticFunc = null;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets a value indicating whether the Func's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                if (_staticFunc == null && Reference == null)
                {
                    return false;
                }
                if (_staticFunc == null)
                {
                    return Reference.IsAlive;
                }
                return Reference == null || Reference.IsAlive;
            }
        }

        /// <summary>
        /// Get a value indicating whether the WeakFunc is static or not.
        /// </summary>
        public bool IsStatic => _staticFunc != null;

        /// <summary>
        /// Gets the name of the method that this WeakFunc represents.
        /// </summary>
        public virtual string MethodName => _staticFunc != null ? _staticFunc.Method.Name : Method.Name;

        /// <summary>
        /// Gets the Func's owner. This object is stored as a
        /// <see cref="WeakReference" />.
        /// </summary>
        public object Target => Reference?.Target;

        /// <summary>
        /// Gets or sets a WeakReference to this WeakFunc's action's target.
        /// This is not necessarily the same as
        /// <see cref="Reference" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected WeakReference FuncReference { get; set; }

        /// <summary>
        /// Gets the owner of the Func that was passed as parameter.
        /// This is not necessarily the same as
        /// <see cref="Target" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected object FuncTarget => FuncReference?.Target;

        /// <summary>
        /// Gets or sets the <see cref="MethodInfo" /> corresponding to this WeakFunc's
        /// method passed in the constructor.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        /// Gets or sets a WeakReference to the target passed when constructing
        /// the WeakFunc. This is not necessarily the same as
        /// <see cref="FuncReference" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected WeakReference Reference { get; set; }

        #endregion
    }

    /// <summary>
    /// Stores an Func without causing a hard reference
    /// to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Func's parameter.</typeparam>
    /// <typeparam name="TResult">The type of the Func's return value.</typeparam>
    public class WeakFunc<T, TResult> : WeakFunc<TResult>, IExecuteWithObjectAndResult
    {
        #region member vars

        private Func<T, TResult> _staticFunc;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="func">The Func that will be associated to this instance.</param>
        public WeakFunc(Func<T, TResult> func) : this(func?.Target, func)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="target">The Func's owner.</param>
        /// <param name="func">The Func that will be associated to this instance.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public WeakFunc(object target, Func<T, TResult> func)
        {
            if (func.Method.IsStatic)
            {
                _staticFunc = func;
                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }
                return;
            }
            Method = func.Method;
            FuncReference = new WeakReference(func.Target);
            Reference = new WeakReference(target);
        }

        #endregion

        #region explicit interfaces

        /// <summary>
        /// Executes the Func with a parameter of type object. This parameter
        /// will be casted to T. This method implements <see cref="IExecuteWithObject.ExecuteWithObject" />
        /// and can be useful if you store multiple WeakFunc{T} instances but don't know in advance
        /// what type T represents.
        /// </summary>
        /// <param name="parameter">
        /// The parameter that will be passed to the Func after
        /// being casted to T.
        /// </param>
        /// <returns>The result of the execution as object, to be casted to T.</returns>
        public object ExecuteWithObject(object parameter)
        {
            var parameterCasted = (T)parameter;
            return Execute(parameterCasted);
        }

        #endregion

        #region methods

        /// <summary>
        /// Executes the Func. This only happens if the Func's owner
        /// is still alive. The Func's parameter is set to default(T).
        /// </summary>
        /// <returns>The result of the Func stored as reference.</returns>
        public new TResult Execute()
        {
            return Execute(default(T));
        }

        /// <summary>
        /// Executes the Func. This only happens if the Func's owner
        /// is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        /// <returns>The result of the Func stored as reference.</returns>
        public TResult Execute(T parameter)
        {
            if (_staticFunc != null)
            {
                return _staticFunc(parameter);
            }
            var funcTarget = FuncTarget;
            if (!IsAlive)
            {
                return default(TResult);
            }
            if (Method != null && FuncReference != null && funcTarget != null)
            {
                return (TResult)Method.Invoke(funcTarget, new object[] { parameter });
            }
            return default(TResult);
        }

        /// <summary>
        /// Sets all the funcs that this WeakFunc contains to null,
        /// which is a signal for containing objects that this WeakFunc
        /// should be deleted.
        /// </summary>
        public new void MarkForDeletion()
        {
            _staticFunc = null;
            base.MarkForDeletion();
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets a value indicating whether the Func's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if (_staticFunc == null && Reference == null)
                {
                    return false;
                }
                if (_staticFunc == null)
                {
                    return Reference.IsAlive;
                }
                return Reference == null || Reference.IsAlive;
            }
        }

        /// <summary>
        /// Gets or sets the name of the method that this WeakFunc represents.
        /// </summary>
        public override string MethodName => _staticFunc != null ? _staticFunc.Method.Name : Method.Name;

        #endregion
    }
}