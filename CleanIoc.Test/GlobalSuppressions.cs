/*
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This interface exists only to demonstrate the framework working rather than any implementation reasons", Scope = "type", Target = "~T:CleanIoc.Core.Test.DummyTypes.Interfaces.IFourthInterfaceActuallyDoesntHaveAnyDerivedClasses")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This interface exists only to demonstrate the framework working rather than any implementation reasons", Scope = "type", Target = "~T:CleanIoc.Core.Test.DummyTypes.Interfaces.ISimpleInterface")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This interface exists only to demonstrate the framework working rather than any implementation reasons", Scope = "type", Target = "~T:CleanIoc.Core.Test.DummyTypes.Interfaces.IThirdInterface")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Tests cannot be static", Scope = "member", Target = "~M:CleanIoc.Core.Test.TestContainer.TestExceptionThatIsThrowHasHierarchyInformationIfCantFullySatisfyConstructor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Tests cannot be static", Scope = "member", Target = "~M:CleanIoc.Core.Test.TestContainer.TestRegistryIsFoundAutomatically")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Tests cannot be static", Scope = "member", Target = "~M:CleanIoc.Core.Test.TestContainer.TestThatItCanConstructTypesWithASingleRegistredType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Tests cannot be static", Scope = "member", Target = "~M:CleanIoc.Core.Test.TestContainer.TestThatItWorksWithASimpleTypeWithADefaultConstructor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Tests cannot be static", Scope = "member", Target = "~M:CleanIoc.Core.Test.TestContainer.TestThatWithoutSettingALifetimeItIsTransient")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Tests cannot be static", Scope = "member", Target = "~M:CleanIoc.Core.Test.TestContainer.TestThrowsAnExceptionIfCantFullySatisfyConstructor")]