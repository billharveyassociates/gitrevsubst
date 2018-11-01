pipeline {
  agent any
  stages {
    stage('build') {
      steps {
        bat '"\\"${tool \'MSBuild\'}\\" gitrevsubst.sln /p:Configuration=Release /p:Platform=\\"Any CPU\\""'
      }
    }
  }
}