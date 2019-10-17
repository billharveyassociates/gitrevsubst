pipeline {
  agent any
  stages {
    stage('build') {
      steps {
          script {
			bat '%MSBUILD15% gitrevsubst.sln /p:Configuration=Release'
			def incremental = gitChangelog from: [type: 'COMMIT', value: "${GIT_PREVIOUS_COMMIT}"], returnType: 'STRING', template: readFile "${env.WORKSPACE}/changelog/incremental_template.txt", to: [type: 'COMMIT', value: "${GIT_COMMIT}"]
			currentBuild.description = incremental
		  }
      }
    }
  }
}