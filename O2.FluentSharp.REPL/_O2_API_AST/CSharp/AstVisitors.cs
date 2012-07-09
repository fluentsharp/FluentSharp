using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;


namespace O2.API.AST.CSharp
{
    public class AstVisitors : AbstractAstVisitor
    {
        public List<LocalVariableDeclaration> localVariableDeclarations = new List<LocalVariableDeclaration>();
        public List<InvocationExpression> invocationExpressions = new List<InvocationExpression>();

        public override object VisitLocalVariableDeclaration(LocalVariableDeclaration localVariableDeclaration, object data)
        {
            localVariableDeclarations.Add(localVariableDeclaration);
            return base.VisitLocalVariableDeclaration(localVariableDeclaration, data);
        }

        public override object VisitInvocationExpression(InvocationExpression invocationExpression, object data)
        {
            invocationExpressions.Add(invocationExpression);
            return base.VisitInvocationExpression(invocationExpression, data);
            
        }


        //here are all available  (change virtual to override when copy and implementing above)
        /*
            public virtual object VisitAddHandlerStatement(AddHandlerStatement addHandlerStatement, object data);
            public virtual object VisitAddressOfExpression(AddressOfExpression addressOfExpression, object data);
            public virtual object VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression, object data);
            public virtual object VisitArrayCreateExpression(ArrayCreateExpression arrayCreateExpression, object data);
            public virtual object VisitAssignmentExpression(AssignmentExpression assignmentExpression, object data);
            public virtual object VisitAttribute(Attribute attribute, object data);
            public virtual object VisitAttributeSection(AttributeSection attributeSection, object data);
            public virtual object VisitBaseReferenceExpression(BaseReferenceExpression baseReferenceExpression, object data);
            public virtual object VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression, object data);
            public virtual object VisitBlockStatement(BlockStatement blockStatement, object data);
            public virtual object VisitBreakStatement(BreakStatement breakStatement, object data);
            public virtual object VisitCaseLabel(CaseLabel caseLabel, object data);
            public virtual object VisitCastExpression(CastExpression castExpression, object data);
            public virtual object VisitCatchClause(CatchClause catchClause, object data);
            public virtual object VisitCheckedExpression(CheckedExpression checkedExpression, object data);
            public virtual object VisitCheckedStatement(CheckedStatement checkedStatement, object data);
            public virtual object VisitClassReferenceExpression(ClassReferenceExpression classReferenceExpression, object data);
            public virtual object VisitCollectionInitializerExpression(CollectionInitializerExpression collectionInitializerExpression, object data);
            public virtual object VisitCompilationUnit(CompilationUnit compilationUnit, object data);
            public virtual object VisitConditionalExpression(ConditionalExpression conditionalExpression, object data);
            public virtual object VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration, object data);
            public virtual object VisitConstructorInitializer(ConstructorInitializer constructorInitializer, object data);
            public virtual object VisitContinueStatement(ContinueStatement continueStatement, object data);
            public virtual object VisitDeclareDeclaration(DeclareDeclaration declareDeclaration, object data);
            public virtual object VisitDefaultValueExpression(DefaultValueExpression defaultValueExpression, object data);
            public virtual object VisitDelegateDeclaration(DelegateDeclaration delegateDeclaration, object data);
            public virtual object VisitDestructorDeclaration(DestructorDeclaration destructorDeclaration, object data);
            public virtual object VisitDirectionExpression(DirectionExpression directionExpression, object data);
            public virtual object VisitDoLoopStatement(DoLoopStatement doLoopStatement, object data);
            public virtual object VisitElseIfSection(ElseIfSection elseIfSection, object data);
            public virtual object VisitEmptyStatement(EmptyStatement emptyStatement, object data);
            public virtual object VisitEndStatement(EndStatement endStatement, object data);
            public virtual object VisitEraseStatement(EraseStatement eraseStatement, object data);
            public virtual object VisitErrorStatement(ErrorStatement errorStatement, object data);
            public virtual object VisitEventAddRegion(EventAddRegion eventAddRegion, object data);
            public virtual object VisitEventDeclaration(EventDeclaration eventDeclaration, object data);
            public virtual object VisitEventRaiseRegion(EventRaiseRegion eventRaiseRegion, object data);
            public virtual object VisitEventRemoveRegion(EventRemoveRegion eventRemoveRegion, object data);
            public virtual object VisitExitStatement(ExitStatement exitStatement, object data);
            public virtual object VisitExpressionRangeVariable(ExpressionRangeVariable expressionRangeVariable, object data);
            public virtual object VisitExpressionStatement(ExpressionStatement expressionStatement, object data);
            public virtual object VisitExternAliasDirective(ExternAliasDirective externAliasDirective, object data);
            public virtual object VisitFieldDeclaration(FieldDeclaration fieldDeclaration, object data);
            public virtual object VisitFixedStatement(FixedStatement fixedStatement, object data);
            public virtual object VisitForeachStatement(ForeachStatement foreachStatement, object data);
            public virtual object VisitForNextStatement(ForNextStatement forNextStatement, object data);
            public virtual object VisitForStatement(ForStatement forStatement, object data);
            public virtual object VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement, object data);
            public virtual object VisitGotoStatement(GotoStatement gotoStatement, object data);
            public virtual object VisitIdentifierExpression(IdentifierExpression identifierExpression, object data);
            public virtual object VisitIfElseStatement(IfElseStatement ifElseStatement, object data);
            public virtual object VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration, object data);
            public virtual object VisitIndexerExpression(IndexerExpression indexerExpression, object data);
            public virtual object VisitInnerClassTypeReference(InnerClassTypeReference innerClassTypeReference, object data);
            public virtual object VisitInterfaceImplementation(InterfaceImplementation interfaceImplementation, object data);            
            public virtual object VisitLabelStatement(LabelStatement labelStatement, object data);
            public virtual object VisitLambdaExpression(LambdaExpression lambdaExpression, object data);            
            public virtual object VisitLockStatement(LockStatement lockStatement, object data);
            public virtual object VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression, object data);
            public virtual object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data);
            public virtual object VisitNamedArgumentExpression(NamedArgumentExpression namedArgumentExpression, object data);
            public virtual object VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration, object data);
            public virtual object VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression, object data);
            public virtual object VisitOnErrorStatement(OnErrorStatement onErrorStatement, object data);
            public virtual object VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration, object data);
            public virtual object VisitOptionDeclaration(OptionDeclaration optionDeclaration, object data);
            public virtual object VisitParameterDeclarationExpression(ParameterDeclarationExpression parameterDeclarationExpression, object data);
            public virtual object VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression, object data);
            public virtual object VisitPointerReferenceExpression(PointerReferenceExpression pointerReferenceExpression, object data);
            public virtual object VisitPrimitiveExpression(PrimitiveExpression primitiveExpression, object data);
            public virtual object VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration, object data);
            public virtual object VisitPropertyGetRegion(PropertyGetRegion propertyGetRegion, object data);
            public virtual object VisitPropertySetRegion(PropertySetRegion propertySetRegion, object data);
            public virtual object VisitQueryExpression(QueryExpression queryExpression, object data);
            public virtual object VisitQueryExpressionAggregateClause(QueryExpressionAggregateClause queryExpressionAggregateClause, object data);
            public virtual object VisitQueryExpressionDistinctClause(QueryExpressionDistinctClause queryExpressionDistinctClause, object data);
            public virtual object VisitQueryExpressionFromClause(QueryExpressionFromClause queryExpressionFromClause, object data);
            public virtual object VisitQueryExpressionGroupClause(QueryExpressionGroupClause queryExpressionGroupClause, object data);
            public virtual object VisitQueryExpressionGroupJoinVBClause(QueryExpressionGroupJoinVBClause queryExpressionGroupJoinVBClause, object data);
            public virtual object VisitQueryExpressionGroupVBClause(QueryExpressionGroupVBClause queryExpressionGroupVBClause, object data);
            public virtual object VisitQueryExpressionJoinClause(QueryExpressionJoinClause queryExpressionJoinClause, object data);
            public virtual object VisitQueryExpressionJoinConditionVB(QueryExpressionJoinConditionVB queryExpressionJoinConditionVB, object data);
            public virtual object VisitQueryExpressionJoinVBClause(QueryExpressionJoinVBClause queryExpressionJoinVBClause, object data);
            public virtual object VisitQueryExpressionLetClause(QueryExpressionLetClause queryExpressionLetClause, object data);
            public virtual object VisitQueryExpressionLetVBClause(QueryExpressionLetVBClause queryExpressionLetVBClause, object data);
            public virtual object VisitQueryExpressionOrderClause(QueryExpressionOrderClause queryExpressionOrderClause, object data);
            public virtual object VisitQueryExpressionOrdering(QueryExpressionOrdering queryExpressionOrdering, object data);
            public virtual object VisitQueryExpressionPartitionVBClause(QueryExpressionPartitionVBClause queryExpressionPartitionVBClause, object data);
            public virtual object VisitQueryExpressionSelectClause(QueryExpressionSelectClause queryExpressionSelectClause, object data);
            public virtual object VisitQueryExpressionSelectVBClause(QueryExpressionSelectVBClause queryExpressionSelectVBClause, object data);
            public virtual object VisitQueryExpressionWhereClause(QueryExpressionWhereClause queryExpressionWhereClause, object data);
            public virtual object VisitRaiseEventStatement(RaiseEventStatement raiseEventStatement, object data);
            public virtual object VisitReDimStatement(ReDimStatement reDimStatement, object data);
            public virtual object VisitRemoveHandlerStatement(RemoveHandlerStatement removeHandlerStatement, object data);
            public virtual object VisitResumeStatement(ResumeStatement resumeStatement, object data);
            public virtual object VisitReturnStatement(ReturnStatement returnStatement, object data);
            public virtual object VisitSizeOfExpression(SizeOfExpression sizeOfExpression, object data);
            public virtual object VisitStackAllocExpression(StackAllocExpression stackAllocExpression, object data);
            public virtual object VisitStopStatement(StopStatement stopStatement, object data);
            public virtual object VisitSwitchSection(SwitchSection switchSection, object data);
            public virtual object VisitSwitchStatement(SwitchStatement switchStatement, object data);
            public virtual object VisitTemplateDefinition(TemplateDefinition templateDefinition, object data);
            public virtual object VisitThisReferenceExpression(ThisReferenceExpression thisReferenceExpression, object data);
            public virtual object VisitThrowStatement(ThrowStatement throwStatement, object data);
            public virtual object VisitTryCatchStatement(TryCatchStatement tryCatchStatement, object data);
            public virtual object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data);
            public virtual object VisitTypeOfExpression(TypeOfExpression typeOfExpression, object data);
            public virtual object VisitTypeOfIsExpression(TypeOfIsExpression typeOfIsExpression, object data);
            public virtual object VisitTypeReference(TypeReference typeReference, object data);
            public virtual object VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression, object data);
            public virtual object VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression, object data);
            public virtual object VisitUncheckedExpression(UncheckedExpression uncheckedExpression, object data);
            public virtual object VisitUncheckedStatement(UncheckedStatement uncheckedStatement, object data);
            public virtual object VisitUnsafeStatement(UnsafeStatement unsafeStatement, object data);
            public virtual object VisitUsing(Using @using, object data);
            public virtual object VisitUsingDeclaration(UsingDeclaration usingDeclaration, object data);
            public virtual object VisitUsingStatement(UsingStatement usingStatement, object data);
            public virtual object VisitVariableDeclaration(VariableDeclaration variableDeclaration, object data);
            public virtual object VisitWithStatement(WithStatement withStatement, object data);
            public virtual object VisitYieldStatement(YieldStatement yieldStatement, object data);
*/
    }
}
